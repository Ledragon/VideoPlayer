using System;
using System.Diagnostics;
using System.IO;

namespace VideoPlayer.Ffmpeg
{
    public class FfprobeInfoExtractor
    {
        public FfprobeVideoInfo GetVideoInfo(String filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"File '{filePath}' does not exist.");
            }
            var process = new Process();
            process.StartInfo.FileName = "ffprobe";
            process.StartInfo.ArgumentList.Add("-v error");
            process.StartInfo.ArgumentList.Add("-show_format");
            process.StartInfo.ArgumentList.Add("-show_streams");
            process.StartInfo.ArgumentList.Add("-of json");
            process.StartInfo.ArgumentList.Add(filePath);
            process.Start();
            process.WaitForExit();
            using (var stream = process.StandardOutput)
            {
                var text = stream.ReadToEnd();
            }
            return new FfprobeVideoInfo();
            //process.StartInfo.ArgumentList = new List<String> { };
        }
    }
}
