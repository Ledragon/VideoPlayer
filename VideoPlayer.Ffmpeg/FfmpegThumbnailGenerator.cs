using LeDragon.Log.Standard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using VideoPlayer.Helpers;

namespace VideoPlayer.Ffmpeg
{
    public class FfmpegThumbnailGenerator : IFfmpegThumbnailGenerator
    {
        private IFfprobeInfoExtractor _ffprobeInfoExtractor;
        private readonly IPathService _pathService;
        private ILogger _logger;

        public FfmpegThumbnailGenerator(IFfprobeInfoExtractor ffprobeInfoExtractor, IPathService pathService)
        {
            this._ffprobeInfoExtractor = ffprobeInfoExtractor;
            this._pathService = pathService;
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
                    var outputDir = this._pathService.GetThumbnailDirectory();
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
                        
                        Double fps = (Double.Parse(info.format.duration, CultureInfo.InvariantCulture) / count);
                        process.StartInfo.ArgumentList.Add($"fps=1/{fps.ToString(CultureInfo.InvariantCulture)},scale=640:-1");

                        process.StartInfo.ArgumentList.Add($"thumb_{Path.GetFileNameWithoutExtension(videoFilePath)}_%03d.png");
                        process.StartInfo.RedirectStandardOutput = false;
                        process.StartInfo.UseShellExecute = false;
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

        public String GenerateContactSheet(String videoFilePath, Int32 rows, Int32 cols)
        {
            try
            {
                String thumbnails = String.Empty;
                if (File.Exists(videoFilePath))
                {
                    var info = this._ffprobeInfoExtractor.GetVideoInfo(videoFilePath);
                    var outputDir = this._pathService.GetThumbnailDirectory();
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
                        var fileName = $"cs_{Path.GetFileNameWithoutExtension(videoFilePath)}.png";
                        Double fps = (Double.Parse(info.format.duration, CultureInfo.InvariantCulture) / (rows * cols));
                        var process = new Process();
                        process.StartInfo.WorkingDirectory = outputDir;
                        process.StartInfo.FileName = "ffmpeg";
                        process.StartInfo.ArgumentList.Add("-i");
                        process.StartInfo.ArgumentList.Add(videoFilePath);
                        process.StartInfo.ArgumentList.Add("-vf");
                        process.StartInfo.ArgumentList.Add($"fps=1/{fps.ToString(CultureInfo.InvariantCulture)},tile={cols}x{rows},scale=640:-1");
                        process.StartInfo.ArgumentList.Add(fileName);
                        process.StartInfo.RedirectStandardOutput = false;
                        process.StartInfo.UseShellExecute = false;
                        process.Start();
                        process.WaitForExit();
                        thumbnails = Path.Combine(outputDir, fileName);
                    }
                }
                return thumbnails;

            }
            catch (Exception e)
            {
                this._logger.Error(e);
                return String.Empty;
            }
        }
    }
}
