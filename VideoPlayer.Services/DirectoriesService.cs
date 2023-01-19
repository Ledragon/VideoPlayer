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
            // return @"E:\Hugues Stefanski\Documents\Development\GitHub\VideoPlayer\VideoPlayer\bin\Debug\Files\Library.xml";
            return @"/home/hugues/Documents/Code/files/Library.xml";
        }
    }
}
