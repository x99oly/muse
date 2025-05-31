using Muse.Src.Entities;
using Muse.Src.Interfaces;
using System.Text.Json;

namespace Muse.Src.Factories
{
    class PlaylistFactory : IFactory<Playlist, string>
    {
        private readonly ILog _logger;

        public PlaylistFactory(ILog logger)
        {
            _logger = logger;
        }

        public Playlist Create(string json)
        {
            var logStart = new LoggerInfo
            {
                Caller = "PlaylistFactory/Create",
                Message = "Starting parsing JSON to Playlist"
            };
            _logger.Debug(logStart);

            using JsonDocument playlistResponse = JsonDocument.Parse(json);
            JsonElement item = playlistResponse.RootElement.GetProperty("items")[0];
            JsonElement snippet = item.GetProperty("snippet");
            JsonElement thumbnails = snippet.GetProperty("thumbnails");

            List<Thumb> thumbs = new List<Thumb>();

            foreach (JsonProperty thumbProp in thumbnails.EnumerateObject())
            {
                JsonElement thumbElement = thumbProp.Value;

                string url = thumbElement.GetProperty("url").GetString() ?? "";
                string width = thumbElement.TryGetProperty("width", out var w) ? w.GetInt32().ToString() : "0";
                string height = thumbElement.TryGetProperty("height", out var h) ? h.GetInt32().ToString() : "0";

                thumbs.Add(new Thumb(url, width, height));
            }

            var playlist = new Playlist(
                id: item.GetProperty("id").GetString() ?? "",
                title: snippet.GetProperty("title").GetString() ?? "",
                description: snippet.GetProperty("description").GetString() ?? "",
                channelId: snippet.GetProperty("channelId").GetString() ?? "",
                channelTitle: snippet.GetProperty("channelTitle").GetString() ?? "",
                publishedAt: snippet.GetProperty("publishedAt").GetDateTime(),
                thumbs: thumbs
            );

            var logEnd = new LoggerInfo
            {
                Caller = "PlaylistFactory/Create",
                Message = $"Finished parsing JSON. Created Playlist with id: {playlist.Id}, title: {playlist.Title}"
            };
            _logger.Info(logEnd);

            return playlist;
        }
    }
}
