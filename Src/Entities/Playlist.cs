namespace Muse.Src.Entities
{
    public class Playlist
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ChannelId { get; set; } = null!;
        public string ChannelTitle { get; set; } = null!;
        public DateTime PublishedAt { get; set; }

        // URLs das thumbnails em diferentes resoluções
        public string ThumbnailDefaultUrl { get; set; } = null!;
        public string ThumbnailMediumUrl { get; set; } = null!;
        public string ThumbnailHighUrl { get; set; } = null!;
        public string ThumbnailStandardUrl { get; set; } = null!;
        public string ThumbnailMaxresUrl { get; set; } = null!;
        
        public override string ToString()
        {
            return $"Playlist: {Title} ({Id})\n" +
                   $"Channel: {ChannelTitle} ({ChannelId})\n" +
                   $"Published: {PublishedAt:yyyy-MM-dd}\n" +
                   $"Description: {Description}\n" +
                   $"Thumbnails:\n" +
                   $"  Default: {ThumbnailDefaultUrl}\n" +
                   $"  Medium: {ThumbnailMediumUrl}\n" +
                   $"  High: {ThumbnailHighUrl}\n" +
                   $"  Standard: {ThumbnailStandardUrl}\n" +
                   $"  Maxres: {ThumbnailMaxresUrl}";
        }

    }
}
