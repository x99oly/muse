using Muse.Src.Extensions;
using YoutubeExplode;
using YoutubeExplode.Converter;
using Muse.Src.Entities;
using System.IO;

namespace Muse.Src.Services
{
    public class MusicDownloadService
    {
        private readonly YoutubeClient _youtube = new YoutubeClient();
        private readonly string _musicsFolder;

        public MusicDownloadService()
        {
            string basePath = Directory.GetCurrentDirectory();
            string projectRoot = Path.Combine(basePath, @""); 
            _musicsFolder = Path.GetFullPath(Path.Combine(projectRoot, "public", "musics"));

            CreateFolder();
        }

        private void CreateFolder()
        {
            if (!Directory.Exists(_musicsFolder))
            {
                Directory.CreateDirectory(_musicsFolder);
                Console.WriteLine($"Pasta criada: {_musicsFolder}");
            }
        }

        public async Task DownloadMusicAsync(Music music)
        {
            string url = $"https://youtube.com/watch?v={music.VideoId}";
            string title = music.Title.SanitizeFileName();
            string outputPath = Path.Combine(_musicsFolder, $"{title}.mp3");

            if (File.Exists(outputPath))
            {
                Console.WriteLine($"{music.Title} - already downloaded. Skipping.");
                return;
            }

            try
            {
                await _youtube.Videos.DownloadAsync(url, outputPath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Downloader failed in proccess of {music.Title}: {e.Message}");
            }
            Console.WriteLine($"{music.Title} - successfully downloaded in {outputPath}.");
        }
    }
}
