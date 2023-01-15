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
            process.StartInfo.ArgumentList.Add("-v");
            process.StartInfo.ArgumentList.Add("error");
            process.StartInfo.ArgumentList.Add("-show_format");
            process.StartInfo.ArgumentList.Add("-show_streams");
            process.StartInfo.ArgumentList.Add("-of");
            process.StartInfo.ArgumentList.Add("json");
            process.StartInfo.ArgumentList.Add(filePath);
            process.StartInfo.RedirectStandardOutput = true;
            //process.StartInfo.UseShellExecute= true;   
            process.Start();
            process.WaitForExit();
            var text = process.StandardOutput.ReadToEnd();
            return new FfprobeVideoInfo();
            //process.StartInfo.ArgumentList = new List<String> { };
        }
    }
}
