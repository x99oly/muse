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

        private MusicDownloadService _downloader;

        public YoutubeApiClient(HttpClient client, string apiKey, MusicDownloadService downloader)
        {
            _client = client;
            _apiKey = apiKey;
            _downloader = downloader;
        }

        public async Task GetAndSaveMusic(string url)
        {
            bool isPlaylist;
            string id = GetIdFromUrl(url, out isPlaylist);

            if (!isPlaylist)
            {
                Music music = await GetMusicAsync(id);
                await _downloader.DownloadMusicAsync(music);
            }
            else
            {
                List<Music> musics = await GetListOfMusic(id);   
            }
        }

        private async Task<Music> GetMusicAsync(string id)
        {
            string url = $"https://www.googleapis.com/youtube/v3/videos?part=snippet&id={id}&key={_apiKey}";
            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return new MusicFactory().Create(content).FirstOrDefault()!;
        }

        private async Task<List<Music>> GetListOfMusic(string id)
        {
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

            return allMusic;
        }

        private async Task<List<Music>> GetListOfMusic(string json, Playlist playlist)
        {
            MusicFactory mf = new MusicFactory();
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
            string url = $"https://www.googleapis.com/youtube/v3/playlists?part=snippet&id={id}&key={_apiKey}";
            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return new PlaylistFactory().Create(content);
        }

        private string GetIdFromUrl(string url, out bool isPlaylist)
        {
            string? res = ExtractIdFromUrl(url, out isPlaylist);
            if (res is null) throw new ArgumentException("Unable to retrieve video ID.");

            return res;
        }

        private string? ExtractIdFromUrl(string url, out bool isPlaylist)
        {
            if (String.IsNullOrWhiteSpace(url))
                throw new InvalidOperationException("Url wasn't informed.");
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
                throw new UriFormatException($"Parse of url failed: {e.Message}");
            }
        }

    }
}



