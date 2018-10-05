using Classes;
using System;
using System.Collections.Generic;
using System.IO;
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
                    new Thumb { Preview = video.FileName+".png" }
                },
                LastPlayed = video.LastPlayed,
                OriginalTitle = Path.GetFileNameWithoutExtension(video.FileName),
                Title = video.Title,
                PlayCount = video.NumberOfViews,
                Runtime = (Int32)Math.Floor(video.Length.TotalMinutes),
                Thumb = new Thumb
                {
                    Preview = video.GetThumbPath(),
                    Aspect = "poster"
                }
            };
            return res;
        }

        public static String GetThumbPath(this Video video)
        {
            var fileName = Path.GetFileNameWithoutExtension(video.FileName);
            var thumbName = Path.Combine(new FileInfo(video.FileName).DirectoryName, fileName + ".preview.png");
            return thumbName;
        }
    }
}
