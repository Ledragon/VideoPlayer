using System.Collections.Generic;
using Classes;
using VideoPlayer.Entities;

namespace VideoPlayer.Services
{
    public interface IPlaylistService
    {
        IEnumerable<Entities.Video> Playlist { get; set; }
        void Clear();
    }
}