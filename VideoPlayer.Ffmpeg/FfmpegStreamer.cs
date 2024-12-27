
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
    // Add an optional parameter to only stream a sepcified amount in bytes
    public async Task<System.IO.Stream?> Stream(String videoPath, Int64 startByte)
    {
      if (this._activeProcess.Any(p => !p.HasExited))
      {
        this._activeProcess.ForEach(p => p.Kill());
        this._activeProcess.Clear();
      }
      var process = new Process
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "ffmpeg",
          Arguments = $"-i \"{videoPath}\" -c:v libx264 -preset veryfast -c:a aac -f mp4 -movflags frag_keyframe+empty_moov pipe:1",
          RedirectStandardOutput = true,
          UseShellExecute = false,
          CreateNoWindow = true
        }
      };

      try
      {
        process.Start();
        this._activeProcess.Add(process);
        var stream = process.StandardOutput.BaseStream;

        if (startByte > 0)
        {
          // Skip to the requested position
          await stream.ReadAsync(new Byte[startByte], 0, (Int32)startByte);
        }
        return stream;
      }
      catch (Exception ex)
      {
        this.Logger().Error(ex);
        throw;
      }

    }
  }
}