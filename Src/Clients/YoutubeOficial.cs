using System.Diagnostics;
using Muse.Src.Interfaces;
using Muse.Src.Entities;
using System.Text.Json;
using Muse.Src.Factories;
using System.Web;
using Muse.Src.Services;

namespace Muse.Src.Clients
{
    public class YoutubeApiClient
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly MusicDownloadService _downloader;
        private readonly ILog _logger;

        public YoutubeApiClient(HttpClient client, string apiKey, MusicDownloadService downloader, ILog logger)
        {
            _client = client;
            _apiKey = apiKey;
            _downloader = downloader;
            _logger = logger;
        }

        public async Task GetAndSaveMusic(string url)
        {
            Stopwatch sw = new();

            var log = new LoggerInfo
            {
                Caller = "YoutubeApiClient/GetAndSaveMusic",
                Message = $"Starting processing for URL: {url}"
            };
            _logger.Info(log);

            bool isPlaylist;
            string id = GetIdFromUrl(url, out isPlaylist);

            log = new LoggerInfo
            {
                Caller = "YoutubeApiClient/GetAndSaveMusic",
                Message = isPlaylist ? $"Detected playlist with ID: {id}" : $"Detected single video with ID: {id}"
            };
            _logger.Info(log);

            try
            {
                if (!isPlaylist)
                {
                    Music music = await GetMusicAsync(id);

                    sw.Start();
                    await _downloader.DownloadMusicAsync(music);
                    sw.Stop();

                    log = new LoggerInfo
                    {
                        Caller = "YoutubeApiClient/GetAndSaveMusic",
                        Message = $"Downloaded single video: {music.Title}, ElapsedTime: {sw.ElapsedMilliseconds} ms"
                    };
                    _logger.Info(log);
                }
                else
                {
                    List<Music> musics = await GetListOfMusic(id);

                    log = new LoggerInfo
                    {
                        Caller = "YoutubeApiClient/GetAndSaveMusic",
                        Message = $"Downloaded {musics.Count} videos from playlist."
                    };
                    _logger.Info(log);
                }
            }
            catch (Exception ex)
            {
                var errorLog = new LoggerInfo
                {
                    Caller = "YoutubeApiClient/GetAndSaveMusic",
                    Message = $"Error while processing URL: {ex.Message}"
                };
                _logger.Error(errorLog);
                throw;
            }
        }

        private async Task<Music> GetMusicAsync(string id)
        {
            var log = new LoggerInfo
            {
                Caller = "YoutubeApiClient/GetMusicAsync",
                Message = $"Fetching video details for ID: {id}"
            };
            _logger.Debug(log);

            string url = $"https://www.googleapis.com/youtube/v3/videos?part=snippet&id={id}&key={_apiKey}";
            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            var music = new MusicFactory(_logger).Create(content).FirstOrDefault();

            if (music == null)
            {
                var warnLog = new LoggerInfo
                {
                    Caller = "YoutubeApiClient/GetMusicAsync",
                    Message = $"No video found for ID: {id}"
                };
                _logger.Warn(warnLog);
            }

            return music!;
        }

        private async Task<List<Music>> GetListOfMusic(string id)
        {
            var log = new LoggerInfo
            {
                Caller = "YoutubeApiClient/GetListOfMusic",
                Message = $"Fetching playlist details for ID: {id}"
            };
            _logger.Debug(log);

            Playlist playlist = await GetPlaylistAsync(id);
            var allMusic = new List<Music>();
            string? nextPageToken = null;

            do
            {
                string url = $"https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&playlistId={playlist.Id}&maxResults=50&key={_apiKey}";

                if (!string.IsNullOrEmpty(nextPageToken))
                    url += $"&pageToken={nextPageToken}";

                var response = await _client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                var musicBatch = await GetListOfMusic(content, playlist);
                allMusic.AddRange(musicBatch);

                using JsonDocument doc = JsonDocument.Parse(content);
                nextPageToken = doc.RootElement.TryGetProperty("nextPageToken", out var tokenElem)
                    ? tokenElem.GetString()
                    : null;

            } while (!string.IsNullOrEmpty(nextPageToken));

            var infoLog = new LoggerInfo
            {
                Caller = "YoutubeApiClient/GetListOfMusic",
                Message = $"Completed fetching playlist with {allMusic.Count} videos."
            };
            _logger.Info(infoLog);

            return allMusic;
        }

        private async Task<List<Music>> GetListOfMusic(string json, Playlist playlist)
        {
            var log = new LoggerInfo
            {
                Caller = "YoutubeApiClient/GetListOfMusic(json, playlist)",
                Message = $"Processing batch of videos from playlist {playlist.Id}"
            };
            _logger.Debug(log);

            MusicFactory mf = new MusicFactory(_logger);
            List<Music> musics = mf.Create(json);

            foreach (var m in musics)
            {
                m.AddToPlaylist(playlist);
                await _downloader.DownloadMusicAsync(m);
            }

            return musics;
        }

        private async Task<Playlist> GetPlaylistAsync(string id)
        {
            var log = new LoggerInfo
            {
                Caller = "YoutubeApiClient/GetPlaylistAsync",
                Message = $"Fetching playlist metadata for ID: {id}"
            };
            _logger.Debug(log);

            string url = $"https://www.googleapis.com/youtube/v3/playlists?part=snippet&id={id}&key={_apiKey}";
            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return new PlaylistFactory(_logger).Create(content);
        }

        private string GetIdFromUrl(string url, out bool isPlaylist)
        {
            string? res = ExtractIdFromUrl(url, out isPlaylist);
            if (res is null)
            {
                var errorLog = new LoggerInfo
                {
                    Caller = "YoutubeApiClient/GetIdFromUrl",
                    Message = $"Unable to retrieve video ID from URL: {url}"
                };
                _logger.Error(errorLog);

                throw new ArgumentException("Unable to retrieve video ID.");
            }

            return res;
        }

        private string? ExtractIdFromUrl(string url, out bool isPlaylist)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                var errorLog = new LoggerInfo
                {
                    Caller = "YoutubeApiClient/ExtractIdFromUrl",
                    Message = "URL was not informed."
                };
                _logger.Error(errorLog);

                throw new InvalidOperationException("Url wasn't informed.");
            }

            isPlaylist = false;
            try
            {
                Uri uri = new Uri(url.Trim());
                var queryParams = HttpUtility.ParseQueryString(uri.Query);
                string? res = queryParams["list"];

                if (res is not null) isPlaylist = true;

                return res ?? queryParams["v"] ?? null;
            }
            catch (UriFormatException e)
            {
                var errorLog = new LoggerInfo
                {
                    Caller = "YoutubeApiClient/ExtractIdFromUrl",
                    Message = $"Parse of url failed: {e.Message}"
                };
                _logger.Error(errorLog);

                throw new UriFormatException($"Parse of url failed: {e.Message}");
            }
        }
    }
}
