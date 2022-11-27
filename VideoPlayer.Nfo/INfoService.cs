using System.Collections.Generic;
using Classes;
using VideoPlayer.Entities;

namespace VideoPlayer.Nfo
{
    public interface INfoService
    {
        void CreateNfo(IEnumerable<Entities.Video> videos);
        void CreateNfo(Video video);
    }
}