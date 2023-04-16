using System;
using System.IO;
using VideoPlayer.Entities;

namespace VideoPlayer.Services
{
    public class PathReplacer
    {
        public void ReplaceDirectory(String sourceDir, String targetDir, Video v, String sourceSep = "\\")
        {
            var targetSep = Path.DirectorySeparatorChar.ToString();
            if (!String.IsNullOrEmpty(sourceDir)
            && !String.IsNullOrEmpty(targetDir)
            && sourceDir != targetDir)
            {
                v.DirectoryPath = v.DirectoryPath?
                    .Replace(sourceDir, targetDir)
                    .Replace(sourceSep, targetSep);
                v.FileName = v.FileName
                    .Replace(sourceDir, targetDir)
                    .Replace(sourceSep, targetSep);
            }
        }
    }
}
