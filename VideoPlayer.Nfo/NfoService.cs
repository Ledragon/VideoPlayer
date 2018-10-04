using Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayer.Nfo
{
    public class NfoService
    {
        public void CreateNfo(IEnumerable<Video> videos)
        {
            foreach (var video in videos)
            {
                var fileName = video.FileName;

            }
        }
    }
}
