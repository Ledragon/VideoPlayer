using System;
using System.IO;

namespace VideoPlayer.Helpers
{
    public class FileSystemHelper
    {

        public static String GetDefaultFileName()
        {
            String defaultFolder = GetDefaultFolder();
            String libraryPath = Path.Combine(defaultFolder, "Library.xml");
            return libraryPath;
        }

        public static String GetDefaultFolder()
        {
            String myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            String videoPlayerPath = Path.Combine(myDocumentsPath, "VideoPlayer");
            if (!Directory.Exists(videoPlayerPath))
            {
                Directory.CreateDirectory(videoPlayerPath);
            }
            return videoPlayerPath;
        }
    }
}
