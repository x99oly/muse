using Muse.Src.Interfaces;
using Muse.Src.Entities;
using System.Text.Json;

namespace Muse.Src.Factories
{
    class PlaylistFactory : IFactory<Playlist, string>
    {
        public Playlist Create(string json)
        {
            using JsonDocument playlistResponse = JsonDocument.Parse(json);
            JsonElement item = playlistResponse.RootElement.GetProperty("items")[0];
            JsonElement snippet = item.GetProperty("snippet");
            JsonElement thumbnails = snippet.GetProperty("thumbnails");

            return new Playlist
            {
                Id = item.GetProperty("id").GetString()!,
                Title = snippet.GetProperty("title").GetString()!,
                Description = snippet.GetProperty("description").GetString()!,
                ChannelId = snippet.GetProperty("channelId").GetString()!,
                ChannelTitle = snippet.GetProperty("channelTitle").GetString()!,
                PublishedAt = snippet.GetProperty("publishedAt").GetDateTime(),
                ThumbnailDefaultUrl = thumbnails.GetProperty("default").GetProperty("url").GetString()!,
                ThumbnailMediumUrl = thumbnails.GetProperty("medium").GetProperty("url").GetString()!,
                ThumbnailHighUrl = thumbnails.GetProperty("high").GetProperty("url").GetString()!,
                ThumbnailStandardUrl = thumbnails.TryGetProperty("standard", out JsonElement standard) ? standard.GetProperty("url").GetString()! : "",
                ThumbnailMaxresUrl = thumbnails.TryGetProperty("maxres", out JsonElement maxres) ? maxres.GetProperty("url").GetString()! : ""
            };
        }
    }
}