using System.Collections.Generic;
using Classes;
using VideoPlayer.Entities;

namespace VideoPlayer.Services
{
    public class PlaylistService : IPlaylistService
    {
        public IEnumerable<Video> Playlist { get; set; }
        public void Clear()
        {
        }
    }
}