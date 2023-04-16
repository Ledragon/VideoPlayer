using System;
using VideoPlayer.Entities;

namespace VideoPlayer.Services
{
    public class PathReplacer
    {
        public void ReplaceDirectory(String sourceDir, String targetDir, Video v, String sourceSep = "\\", String targetSep = "/")
        {
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
