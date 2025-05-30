using Muse.Src.Extensions;
using YoutubeExplode;
using YoutubeExplode.Converter;
using Muse.Src.Entities;

namespace Muse.Src.Services
{
    class MusicDownloadService
    {
        private readonly YoutubeClient _youtube = new YoutubeClient();

        public async Task DownloadMusicAsync(Music music)
        {
            string url = $"https://youtube.com/watch?v={music.VideoId}";
            string title = music.Title.SanitizeFileName();
            string outputPath = $"public/musics/{title}.mp3";

            if (File.Exists(outputPath))
            {
                Console.WriteLine($"{music.Title} - already downloaded. Skipping.");
                return;
            }

            await _youtube.Videos.DownloadAsync(url, outputPath);
            Console.WriteLine($"{music.Title} - successfully downloaded.");
        }
    }
}
