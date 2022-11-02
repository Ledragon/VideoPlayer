using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Classes;

namespace VideoPlayer
{
    internal class InfoExporter
    {
        public static void ExportImageResolutions(IEnumerable<VideoViewModel> videos)
        {
            string path = @"D:\export.csv";
            var streamWriter = new StreamWriter(path);
            streamWriter.WriteLine("Name;Category;Width;Height");
            foreach (VideoViewModel video in videos)
            {
                streamWriter.WriteLine("{0};{3};{1};{2}", video.Title, video.PreviewImage.Width, video.PreviewImage.Height, video.Category);
            }
            streamWriter.Close();
        }
    }
}