using System.Collections.Generic;
using Classes;

namespace VideoPlayer.Nfo
{
    public interface INfoService
    {
        void CreateNfo(IEnumerable<Video> videos);
    }
}