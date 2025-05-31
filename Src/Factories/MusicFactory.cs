using Muse.Src.Interfaces;
using Muse.Src.Entities;
using System.Text.Json;

namespace Muse.Src.Factories
{
    class MusicFactory
    {
        public List<Music> Create(string json)
        {
            var musicList = new List<Music>();

            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement items = doc.RootElement.GetProperty("items");
            Console.WriteLine(items);

            foreach (JsonElement item in items.EnumerateArray())
            {
                JsonElement snippet = item.GetProperty("snippet");
                string videoId;
                if (snippet.TryGetProperty("resourceId", out var resourceId))
                {
                    videoId = snippet.GetProperty("resourceId").GetProperty("videoId").GetString() ?? "";
                }
                else
                {
                    videoId = item.GetProperty("id").GetString() ?? "";   
                }
                string title = snippet.GetProperty("title").GetString() ?? "";
                string publishedAt = snippet.GetProperty("publishedAt").GetString() ?? "";

                // Thumbnails
                List<Thumb> thumbnails = new List<Thumb>();
                if (snippet.TryGetProperty("thumbnails", out JsonElement thumbnailsElement))
                {
                    foreach (JsonProperty thumbProp in thumbnailsElement.EnumerateObject())
                    {
                        JsonElement thumb = thumbProp.Value;
                        string url = thumb.GetProperty("url").GetString() ?? "";
                        string width = thumb.GetProperty("width").GetInt32().ToString();
                        string height = thumb.GetProperty("height").GetInt32().ToString();

                        thumbnails.Add(new Thumb(url, width, height));
                    }
                }

                // Tags
                var tags = new List<string>();
                if (snippet.TryGetProperty("tags", out JsonElement tagsElement) && tagsElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement tag in tagsElement.EnumerateArray())
                    {
                        tags.Add(tag.GetString() ?? "");
                    }
                }

                musicList.Add(new Music(videoId, title, publishedAt, thumbnails, tags));
            }

            return musicList;
        }

    }
}
