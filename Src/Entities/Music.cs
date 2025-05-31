
namespace Muse.Src.Entities
{
    public class Music
    {
        public string VideoId { get; }
        public string Title { get; }
        public string PublishedAt { get; }
        public List<Thumb> Thumbs { get; }
        public List<string> Tags { get; }
        List<Playlist> AssociatedPlaylists = new List<Playlist>();

        public Music(string videoId, string title, string publishedAt, List<Thumb> thumbnails, List<string> tags)
        {
            VideoId = videoId;
            Title = title;
            PublishedAt = publishedAt;
            Thumbs = thumbnails;
            Tags = tags;
        }

        public void AddToPlaylist(Playlist playlist)
        {
            AssociatedPlaylists.Add(playlist);
        }

        public override string ToString()
        {
            return $"Music: {Title} ({VideoId})\n" +
                   $"Published: {PublishedAt}\n" +
                   $"Thumbnails: {Thumbs.Count}\n" +
                   $"Tags: {string.Join(", ", Tags)}";
        }
    }
}

