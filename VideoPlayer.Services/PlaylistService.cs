using System.Collections.Generic;
using Classes;

namespace VideoPlayer.Services
{
    public class PlaylistService : IPlaylistService
    {
        public IEnumerable<Video> Playlist { get; set; } 
    }
}