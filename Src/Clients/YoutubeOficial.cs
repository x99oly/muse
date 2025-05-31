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
            return new MusicFactory().Create(content);
        }

        private async Task<List<Music>> GetListOfMusic(string id)
        {
            Playlist playlist = await GetPlaylistAsync(id);

            var response = await _client.GetAsync(
                $"https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&playlistId={playlist.Id}&maxResults=50&key={_apiKey}"
            );
            var content = await response.Content.ReadAsStringAsync();

            return await GetListOfMusic(content, playlist);
        }

        private async Task<List<Music>> GetListOfMusic(string json, Playlist playlist)
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            JsonElement items = root.GetProperty("items");

            List<Music> musics = new();
            MusicFactory mf = new MusicFactory();

            foreach (JsonElement item in items.EnumerateArray())
            {
                Music m = mf.Create(item.GetRawText());
                m.AddToPlaylist(playlist);
                musics.Add(m);
                await _downloader.DownloadMusicAsync(m);
            }

            return musics;
        }

        private async Task<Playlist> GetPlaylistAsync(string id)
        {
            var response = await _client.GetAsync(
                $"https://www.googleapis.com/youtube/v3/playlists?part=snippet&id={id}&key={_apiKey}"
            );
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



