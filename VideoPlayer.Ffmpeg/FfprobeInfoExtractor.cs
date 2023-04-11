using LeDragon.Log.Standard;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace VideoPlayer.Ffmpeg
{
    public class FfprobeInfoExtractor : IFfprobeInfoExtractor
    {
        private ILogger _logger;

        public FfprobeInfoExtractor()
        {
            this._logger = this.Logger();
        }

        public FfprobeVideoInfo GetVideoInfo(String filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"File '{filePath}' does not exist.");
            }
            try
            {
                this._logger.DebugFormat("Getting video info for '{0}'.", filePath);
                var process = new Process();
                //process.EnableRaisingEvents = true;
                process.StartInfo.FileName = "ffprobe";
                process.StartInfo.ArgumentList.Add("-v");
                process.StartInfo.ArgumentList.Add("error");
                process.StartInfo.ArgumentList.Add("-show_format");
                process.StartInfo.ArgumentList.Add("-show_streams");
                process.StartInfo.ArgumentList.Add("-of");
                process.StartInfo.ArgumentList.Add("json");
                process.StartInfo.ArgumentList.Add(filePath);
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                var isStarted = process.Start();
                if (isStarted)
                {
                    // Wait for 500ms for work to be complete; Deadlock problem if using WaitForExit
                    Thread.Sleep(500);
                    //process.WaitForExit();
                    var text = process.StandardOutput.ReadToEnd();
                    var result = JsonConvert.DeserializeObject<FfprobeVideoInfo>(text);
                    return result;
                }
                else
                {
                    throw new Exception("Process failed to start");
                }

            }
            catch (Exception e)
            {
                this._logger.Error(e);
                throw;
            }
        }
    }
}
