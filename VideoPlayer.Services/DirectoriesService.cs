using System;
using System.IO;

namespace VideoPlayer.Services
{
    public class PathService : IPathService
    {
        public String GetThumbnailDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VideoPlayer", "Temp");
        }

        public String GetLibraryFile()
        {
            return @"/home/hugues/Documents/Code/files/Library.xml";
        }
    }
}
