using LeDragon.Log.Standard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace VideoPlayer.Ffmpeg
{
  public class FfmpegStreamer : IFfmpegStreamer
  {
    private readonly ILogger _logger;
    private readonly List<Process> _activeProcess;

    public FfmpegStreamer()
    {
      this._logger = this.Logger();
      this._activeProcess = new List<Process>();
    }

    public Task<System.IO.Stream?> Stream(String videoPath, Int64 startTimeMs)
    {
      if (this._activeProcess.Any(p => !p.HasExited))
      {
        this._activeProcess.ForEach(p => p.Kill());
        this._activeProcess.Clear();
      }

      var seekArg = startTimeMs > 0 ? $"-ss {(startTimeMs / 1000.0).ToString("0.000")}" : "";

      var process = new Process
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "ffmpeg",
          Arguments = $"{seekArg} -accurate_seek -i \"{videoPath}\" -c:v libx264 -preset veryfast -c:a aac -copyts -avoid_negative_ts make_zero -f mp4 -movflags frag_keyframe+faststart+empty_moov pipe:1",
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          UseShellExecute = false,
          CreateNoWindow = true
        }
      };

      try
      {
        process.Start();
        this._activeProcess.Add(process);

        // Log any ffmpeg errors for debugging
        process.ErrorDataReceived += (sender, e) =>
        {
          if (!String.IsNullOrEmpty(e.Data))
          {
            this._logger.Debug($"FFmpeg: {e.Data}");
          }
        };
        process.BeginErrorReadLine();

        return Task.FromResult<System.IO.Stream?>(process.StandardOutput.BaseStream);
      }
      catch (Exception ex)
      {
        this._logger.Error(ex);
        throw;
      }
    }
  }
}