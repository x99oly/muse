using Muse.Src.Interfaces;
using Muse.Src.Entities;
using System.Text.Json;

namespace Muse.Src.Factories
{
    class MusicFactory : IFactory<Music, string>
    {
        public Music Create(string json)
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement items = doc.RootElement.GetProperty("items");
            JsonElement firstItem = items[0];
            JsonElement snippet = firstItem.GetProperty("snippet");

            string videoId = firstItem.GetProperty("id").GetString() ?? "";
            string title = snippet.GetProperty("title").GetString() ?? "";
            string publishedAt = snippet.GetProperty("publishedAt").GetString() ?? "";

            // Pega todas as thumbnails como dicionário
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

            // Pega as tags (se existirem)
            var tags = new List<string>();
            if (snippet.TryGetProperty("tags", out JsonElement tagsElement) && tagsElement.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement tag in tagsElement.EnumerateArray())
                {
                    tags.Add(tag.GetString() ?? "");
                }
            }

            return new Music(videoId, title, publishedAt, thumbnails, tags);
        }
    }
}
