using System;
using System.Collections.Generic;
using System.IO;

namespace VideoPlayer.Ffmpeg
{
    public class FfmpegThumbnailGenerator
    {
        public List<String> GenerateThumbnails(String videoFilePath, Int32 count)
        {
            var thumbnails = new List<String>();
            if (!File.Exists(videoFilePath))
            {
                var outputDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VideoPlayer", "Temp");
            }
            return thumbnails;
        }
    }
}
