using Muse.Src.Interfaces;
using Muse.Src.Entities;
using System.Text.Json;

namespace Muse.Src.Factories
{
    class MusicFactory: IFactory<Music,string>
    {
        public Music Create(string json)
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement snippet = doc.RootElement.GetProperty("snippet");

            Music m = new Music
            {
                Title = snippet.GetProperty("title").GetString() ?? "",
                Description = snippet.GetProperty("description").GetString() ?? "",
                ChannelId = snippet.GetProperty("channelId").GetString() ?? "",
                ChannelTitle = snippet.GetProperty("channelTitle").GetString() ?? "",
                VideoId = snippet.GetProperty("resourceId").GetProperty("videoId").GetString() ?? "",
                PublishedAt = snippet.GetProperty("publishedAt").GetString() ?? "",
                ThumbnailDefaultUrl = snippet.GetProperty("thumbnails").GetProperty("default").GetProperty("url").GetString() ?? "",
            };
            return m;
        }
    }

}