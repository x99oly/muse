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

            _logger.Info($"Starting processing for URL: {url}");

            bool isPlaylist;
            string id = GetIdFromUrl(url, out isPlaylist);

            _logger.Info(isPlaylist ? $"Detected playlist with ID: {id}" : $"Detected single video with ID: {id}");

            try
            {
                if (!isPlaylist)
                {
                    Music music = await GetMusicAsync(id);

                    sw.Start();
                    await _downloader.DownloadMusicAsync(music);
                    sw.Stop();

                    _logger.Info($"Downloaded single video: {music.Title}, ElapsedTime: {sw.ElapsedMilliseconds} ms");
                }
                else
                {
                    List<Music> musics = await GetListOfMusic(id);

                    _logger.Info($"Downloaded {musics.Count} videos from playlist.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error while processing URL: {ex.Message}", ex);
                throw;
            }
        }

        private async Task<Music> GetMusicAsync(string id)
        {
            _logger.Debug($"Fetching video details for ID: {id}");

            string url = $"https://www.googleapis.com/youtube/v3/videos?part=snippet&id={id}&key={_apiKey}";
            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            var music = new MusicFactory(_logger).Create(content).FirstOrDefault();

            if (music == null)
                _logger.Warn($"No video found for ID: {id}");

            return music!;
        }

        private async Task<List<Music>> GetListOfMusic(string id)
        {
            _logger.Debug($"Fetching playlist details for ID: {id}");

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

            _logger.Info($"Completed fetching playlist with {allMusic.Count} videos.");

            return allMusic;
        }

        private async Task<List<Music>> GetListOfMusic(string json, Playlist playlist)
        {
            _logger.Debug($"Processing batch of videos from playlist {playlist.Id}");

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
            _logger.Debug($"Fetching playlist metadata for ID: {id}");

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
                _logger.Error($"Unable to retrieve video ID from URL: {url}");
                throw new ArgumentException("Unable to retrieve video ID.");
            }

            return res;
        }

        private string? ExtractIdFromUrl(string url, out bool isPlaylist)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                _logger.Error("URL was not informed.");
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
                _logger.Error($"Parse of url failed: {e.Message}");
                throw new UriFormatException($"Parse of url failed: {e.Message}");
            }
        }
    }
}
