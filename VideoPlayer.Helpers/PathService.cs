using System;
using System.IO;
using System.Runtime.InteropServices;

namespace VideoPlayer.Helpers
{
    public class PathService : IPathService
    {
        public String GetThumbnailDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VideoPlayer", "Temp");
        }

        public String GetLibraryFile()
        {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return @"E:\Hugues Stefanski\Documents\Development\GitHub\VideoPlayer\VideoPlayer\bin\Debug\Files\Library.xml";
            }
            else
            {
                return @"/home/hugues/Documents/Code/files/Library.xml";
            }
        }
    }
}
