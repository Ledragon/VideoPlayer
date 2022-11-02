using Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPlayer.Entities;

namespace VideoPlayer.Nfo
{
    public static class VideoNfoExtension
    {
        public static MovieNfo ToNfo(this Entities.Video video)
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
            };
            res.Genres.Add(video.Category);
            if (video.Tags.Any())
            {
                res.Tags.AddRange(video.Tags.Select(t=>t.Value));
            }
            var thumbPath = video.GetThumbPath();
            if (File.Exists(thumbPath))
            {
                res.Thumb = new Thumb
                {
                    Preview = thumbPath,
                    Aspect = "poster"
                };
                res.FanArt.Add(new Thumb { Preview = thumbPath });
            }
            return res;
        }

        public static String GetThumbPath(this Entities.Video video)
        {
            var fileName = Path.GetFileNameWithoutExtension(video.FileName);
            var thumbName = Path.Combine(new FileInfo(video.FileName).DirectoryName, fileName + ".preview.png");
            return thumbName;
        }
    }
}
