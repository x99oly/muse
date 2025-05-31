using Muse.Src.Services;
using Muse.Src.Entities;
using Muse.Src.Clients;

static class Program
{
    public static async Task Main(string[] args)
    {
        string apiKey = GetApiKey();
        string? playlistUrl;
        using var client = new HttpClient();
        var downloader = new MusicDownloadService();
        var youtubeApi = new YoutubeApiClient(client, apiKey, downloader);

        do
        {
            Console.Write("Enter the YouTube URL: ");
            playlistUrl = Console.ReadLine();

        } while (string.IsNullOrWhiteSpace(playlistUrl));

        try
        {
            await youtubeApi.GetAndSaveMusic(playlistUrl);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    public static string GetApiKey()
    {
        string? apiKey = Environment.GetEnvironmentVariable("YOUTUBE_API_KEY");
        if (apiKey is not null) return apiKey;

        TryGetAndSaveApiKey();

        return GetApiKey();
    }

    public static void TryGetAndSaveApiKey()
    {
        string? key = null;
        do
        {
            Console.Write("Could not find youtube api key, please enter it (or press 0[zero] to exit):");
            key = Console.ReadLine();
            if (key == "0")
            {
                Console.WriteLine("Ending program...");
                Environment.Exit(0);
            }
        } while (String.IsNullOrWhiteSpace(key));

        Environment.SetEnvironmentVariable("YOUTUBE_API_KEY", key);
    }

}

