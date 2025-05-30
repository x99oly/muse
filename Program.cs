using Muse.Src.Services;
using Muse.Src.Entities;
using Muse.Src.Clients;

static class Program
{
    public static async Task Main(string[] args)
    {
        Console.Write("Enter the YouTube playlist URL: ");
        string? playlistUrl = Console.ReadLine();

        if (string.IsNullOrEmpty(playlistUrl))
        {
            Console.WriteLine("No valid data was informed.");
            return;
        }

        string playlistId = playlistUrl;
        string? apiKey = Environment.GetEnvironmentVariable("YOUTUBE_API_KEY");

        if (apiKey is null)
        {
            Console.WriteLine("Missing API Key");
            return;
        }

        try
        {
            using var client = new HttpClient();
            var youtubeApi = new YoutubeApiClient(client, apiKey);

            List<Music> musics = await youtubeApi.GetListOfMusic(await youtubeApi.GetPlaylistAsync(playlistId));

            var downloader = new MusicDownloadService();

            foreach (Music music in musics)
            {
                try
                {
                    await downloader.DownloadMusicAsync(music);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

