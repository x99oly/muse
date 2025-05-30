namespace Muse.Src.Entities
{
    public class Music
    {
        public string VideoId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ChannelId { get; set; } = null!;
        public string ChannelTitle { get; set; } = null!;
        public string PlaylistId { get; set; } = null!;
        public int Position { get; set; }
        public string VideoOwnerChannelTitle { get; set; } = null!;
        public string VideoOwnerChannelId { get; set; } = null!;
        public string PublishedAt { get; set; } = null!;
        public string ThumbnailDefaultUrl { get; set; } = null!;
        public string ThumbnailMediumUrl { get; set; } = null!;
        public string ThumbnailHighUrl { get; set; } = null!;

        public List<Playlist> AssociatedPlaylist { get; set; } = new List<Playlist>();
        public override string ToString()
        {
            return $"Music: {Title} ({VideoId})\n" +
                   $"Channel: {ChannelTitle} ({ChannelId})\n" +
                   $"Published: {PublishedAt:yyyy-MM-dd}\n" +
                   $"Description: {Description}\n" +
                   $"Thumbnail: {ThumbnailDefaultUrl}";
        }

        public void AddToPlaylist(Playlist playlist)
        {
            AssociatedPlaylist.Add(playlist);
        }
    }
}
