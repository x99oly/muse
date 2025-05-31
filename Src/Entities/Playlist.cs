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

        public List<Thumb> Thumbs { get; } = new List<Thumb>();
        
        public Playlist(string id, string title, string description, string channelId, string channelTitle, DateTime publishedAt, List<Thumb> thumbs)
        {
            Id = id;
            Title = title;
            Description = description;
            ChannelId = channelId;
            ChannelTitle = channelTitle;
            PublishedAt = publishedAt;
            Thumbs = thumbs;
        }
        public override string ToString()
        {
            return $"Playlist: {Title} ({Id})\n" +
                   $"Channel: {ChannelTitle} ({ChannelId})\n" +
                   $"Published: {PublishedAt:yyyy-MM-dd}\n" +
                   $"Description: {Description}\n" +
                   $"Thumbnails: {Thumbs.Count}";
        }

    }
}
