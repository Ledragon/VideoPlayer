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
            var osNameAndVersion = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            if (osNameAndVersion.ToLower().Contains("windows"))
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
