using Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayer.Nfo
{
    public static class VideoNfoExtension
    {
        public static MovieNfo ToNfo(this Video video)
        {
            var res = new MovieNfo
            {
                DateAdded = video.DateAdded,
                FanArt = new List<Thumb>
                {
                    new Thumb{Preview = video.FileName+".png"}
                }
            };
            return res;
        }
    }
}
