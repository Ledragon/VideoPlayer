using LeDragon.Log.Standard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace VideoPlayer.Ffmpeg
{
    public class FfmpegThumbnailGenerator
    {
        private IFfprobeInfoExtractor _ffprobeInfoExtractor;
        private ILogger _logger;

        public FfmpegThumbnailGenerator(IFfprobeInfoExtractor ffprobeInfoExtractor)
        {
            this._ffprobeInfoExtractor = ffprobeInfoExtractor;
            this._logger = this.Logger();
        }

        public List<String> GenerateThumbnails(String videoFilePath, Int32 count)
        {
            try
            {
                this._logger.DebugFormat($"Generating '{count}' thumbnails for video '{videoFilePath}'.");
                var thumbnails = new List<String>();
                if (File.Exists(videoFilePath))
                {
                    var info = this._ffprobeInfoExtractor.GetVideoInfo(videoFilePath);
                    var outputDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VideoPlayer", "Temp");
                    if (!String.IsNullOrEmpty(outputDir))
                    {
                        if (!Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }
                        var files = Directory.GetFiles(outputDir);
                        if (files.Any())
                        {
                            Array.ForEach(files, f => File.Delete(f));
                        }
                        var process = new Process();
                        process.StartInfo.WorkingDirectory = outputDir;
                        process.StartInfo.FileName = "ffmpeg";
                        process.StartInfo.ArgumentList.Add("-i");
                        process.StartInfo.ArgumentList.Add(videoFilePath);
                        process.StartInfo.ArgumentList.Add("-vf");
                        process.StartInfo.ArgumentList.Add("fps=" + count.ToString() + "/" + info.format.duration);
                        process.StartInfo.ArgumentList.Add($"thumb_{Path.GetFileNameWithoutExtension(videoFilePath)}_%03d.png");
                        process.StartInfo.RedirectStandardOutput = false;
                        process.StartInfo.UseShellExecute = true;
                        process.Start();
                        process.WaitForExit();
                        thumbnails = Directory.GetFiles(outputDir).ToList();
                    }
                }
                return thumbnails;

            }
            catch (Exception e)
            {
                this._logger.Error(e);
                throw;
            }
        }
    }
}
