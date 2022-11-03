using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;

namespace VideoPlayer.Helpers
{
    public class DirectoryHelper
    {
        private static readonly String[] VideoExtensions =
        {
            ".3gp", ".asf", ".avi", ".divx", ".flv", ".m4v", ".mkv", ".mov", ".mp4",
            ".mpeg", ".mpg", ".webm", ".wmv"
        };

        public static List<String> GetVideoFiles(String directoryPath, Boolean isSubDirectoriesIncluded)
        {
            var videoFiles = new List<String>();
            try
            {
                var directoryInfo = new DirectoryInfo(directoryPath);
                SearchOption searchOption = isSubDirectoriesIncluded
                    ? SearchOption.AllDirectories
                    : SearchOption.TopDirectoryOnly;
                videoFiles.AddRange(directoryInfo
                    .GetFiles("*", searchOption)
                    .Where(fileInfo =>
                        Array.IndexOf(VideoExtensions, fileInfo.Extension.ToLowerInvariant()) != -1)
                    .Select(fileInfo => fileInfo.FullName));
            }
            catch (Exception e)
            {
                Logger().Error(e.Message);
                Logger().Error(e.Source);
            }
            return videoFiles;
        }

        private static ILog Logger()
        {
            return LogManager.GetLogger(typeof (DirectoryHelper));
        }
    }
}