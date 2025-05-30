using Muse.Src.Entities;

namespace Muse.Src.Interfaces
{
    public interface IMusicClient
    {
        public Task<Playlist> GetPlaylistAsync(string raw);
        public Task<Music> GetMusicAsync(string raw);

        public Task<List<Music>> GetListOfMusic(Playlist playlist);
    }
}