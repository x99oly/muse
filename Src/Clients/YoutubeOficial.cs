using Muse.Src.Interfaces;
using Muse.Src.Entities;
using System.Text.Json;
using Muse.Src.Factories;

namespace Muse.Src.Clients
{
    public class YoutubeApiClient : IMusicClient
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;

        public YoutubeApiClient(HttpClient client, string apiKey)
        {
            _client = client;
            _apiKey = apiKey;
        }

        public async Task<Music> GetMusicAsync(string playlistId)
        {
            string id = playlistId;
            var response = await _client.GetAsync(
                $"https://www.googleapis.com/youtube/v3/videos?part=snippet&id={id}&key={_apiKey}"
            );
            var content = await response.Content.ReadAsStringAsync();
            return new MusicFactory().Create(content);
        }

        public async Task<List<Music>> GetListOfMusic(Playlist playlist)
        {
            var response = await _client.GetAsync(
                $"https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&playlistId={playlist.Id}&maxResults=50&key={_apiKey}"
            );
            var content = await response.Content.ReadAsStringAsync();

            return GetListOfMusic(content, playlist);
        }

        public async Task<Playlist> GetPlaylistAsync(string url)
        {
            string id = url;
            var response = await _client.GetAsync(
                $"https://www.googleapis.com/youtube/v3/playlists?part=snippet&id={id}&key={_apiKey}"
            );
            var content = await response.Content.ReadAsStringAsync();
            return new PlaylistFactory().Create(content);
        }

        private List<Music> GetListOfMusic(string json, Playlist playlist)
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
            }

            return musics;
        }

    }
}



