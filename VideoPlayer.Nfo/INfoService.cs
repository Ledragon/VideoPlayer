using System.Collections.Generic;
using Classes;
using VideoPlayer.Entities;

namespace VideoPlayer.Nfo
{
    public interface INfoService
    {
        void CreateNfo(IEnumerable<Video> videos);
    }
}