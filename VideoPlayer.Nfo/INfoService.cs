using System;
using System.Collections.Generic;
using VideoPlayer.Entities;

namespace VideoPlayer.Nfo
{
    public interface INfoService
    {
        void CreateNfo(IEnumerable<Video> videos, String outDir = null);
        void CreateNfo(Video video, String outDir = null);
    }
}