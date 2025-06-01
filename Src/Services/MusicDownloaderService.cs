using Muse.Src.Extensions;
using YoutubeExplode;
using YoutubeExplode.Converter;
using Muse.Src.Entities;
using Muse.Src.Interfaces;
using Muse.Src.Handlers;
using System.Diagnostics;

namespace Muse.Src.Services
{
    public class MusicDownloadService
    {
        private readonly YoutubeClient _youtube = new YoutubeClient();
        private readonly FileHandler _fh;
        private readonly ILog _logger;

        public MusicDownloadService(ILog logger, FileHandler fh)
        {
            _logger = logger;
            _fh = fh;
            _fh.CreateDirectory("musics");
        }

        public async Task DownloadMusicAsync(Music music)
        {
            Stopwatch sw = new();

            string url = $"https://youtube.com/watch?v={music.VideoId}";
            string title = music.Title.SanitizeFileName();
            string relativePath = Path.Combine("musics", $"{title}.mp3");

            if (File.Exists(relativePath))
            {
                _logger.Debug($"{music.Title} already downloaded. Skipping.");
                return;
            }

            try
            {
                string path = Path.Combine(_fh.PublicDir, relativePath);
                sw.Start();
                await _youtube.Videos.DownloadAsync(url, path);
                sw.Stop();
                _logger.Info($"{music.Title} successfully downloaded at {relativePath}, ElapsedTime: {sw.Elapsed}");
            }
            catch (Exception e)
            {
                _logger.Error($"Download failed for {music.Title}: {e.Message}", e);
            }
        }
    }

}

