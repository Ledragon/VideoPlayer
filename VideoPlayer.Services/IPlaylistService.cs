using System.Collections.Generic;
using Classes;

namespace VideoPlayer.Services
{
    public interface IPlaylistService
    {
        IEnumerable<Video> Playlist { get; set; }
    }
}