using LeDragon.Log.Standard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VideoPlayer.Ffmpeg
{
  public class AdvancedFfmpegStreamer : IAdvancedFfmpegStreamer
  {
    private readonly ILogger _logger;
    private readonly List<Process> _activeProcesses;
    private readonly SemaphoreSlim _processSemaphore;
    private readonly Dictionary<String, String> _qualityPresets;
    private Boolean? _hardwareAccelerationAvailable;

    public AdvancedFfmpegStreamer()
    {
      this._logger = this.Logger();
      this._activeProcesses = new List<Process>();
      this._processSemaphore = new SemaphoreSlim(4, 4);

      this._qualityPresets = new Dictionary<String, String>
      {
        ["low"] = "-crf 28 -preset fast",
        ["medium"] = "-crf 23 -preset medium",
        ["high"] = "-crf 18 -preset slow",
        ["ultra"] = "-crf 15 -preset veryslow"
      };
    }

    public async Task<System.IO.Stream?> Stream(String videoPath, Int64 startByte)
    {
      return await this.StreamWithSeek(videoPath, 0, 1.0);
    }

    public async Task<System.IO.Stream?> StreamWithSeek(String videoPath, Double seekTimeSeconds, Double playbackSpeed = 1.0, String quality = "medium")
    {
      if (!File.Exists(videoPath))
      {
        this._logger.ErrorFormat("File '{0}' does not exist.", videoPath);
        throw new ArgumentException($"File '{videoPath}' does not exist.");
      }

      await this._processSemaphore.WaitAsync();

      try
      {
        this._logger.DebugFormat("Starting transcoding stream for '{0}' with seek={1}s, speed={2}x, quality={3}",
            videoPath, seekTimeSeconds, playbackSpeed, quality);

        var process = this.CreateTranscodingProcess(videoPath, seekTimeSeconds, playbackSpeed, quality);

        var isStarted = process.Start();
        if (!isStarted)
        {
          this._logger.ErrorFormat("Failed to start FFmpeg process for '{0}'", videoPath);
          throw new Exception("Failed to start FFmpeg process");
        }

        lock (this._activeProcesses)
        {
          this._activeProcesses.Add(process);
        }

        // Monitor process in background
        _ = Task.Run(async () =>
        {
          try
          {
            await Task.Run(() => process.WaitForExit());
            this._logger.DebugFormat("FFmpeg process completed for '{0}'", videoPath);
          }
          catch (Exception ex)
          {
            this._logger.Error(ex);
          }
          finally
          {
            lock (this._activeProcesses)
            {
              this._activeProcesses.Remove(process);
            }
            this._processSemaphore.Release();
          }
        });

        return process.StandardOutput.BaseStream;
      }
      catch (Exception ex)
      {
        this._logger.Error(ex);
        this._processSemaphore.Release();
        throw;
      }
    }

    public async Task<System.IO.Stream?> StreamSegment(String videoPath, Double startTime, Double duration, Double playbackSpeed = 1.0, String quality = "medium")
    {
      if (!File.Exists(videoPath))
      {
        this._logger.ErrorFormat("File '{0}' does not exist.", videoPath);
        throw new ArgumentException($"File '{videoPath}' does not exist.");
      }

      await this._processSemaphore.WaitAsync();

      try
      {
        this._logger.DebugFormat("Starting segment stream for '{0}' from {1}s for {2}s", videoPath, startTime, duration);

        var process = this.CreateSegmentProcess(videoPath, startTime, duration, playbackSpeed, quality);

        var isStarted = process.Start();
        if (!isStarted)
        {
          this._logger.ErrorFormat("Failed to start FFmpeg segment process for '{0}'", videoPath);
          throw new Exception("Failed to start FFmpeg segment process");
        }

        lock (this._activeProcesses)
        {
          this._activeProcesses.Add(process);
        }

        // Monitor process in background
        _ = Task.Run(async () =>
        {
          try
          {
            await Task.Run(() => process.WaitForExit());
          }
          catch (Exception ex)
          {
            this._logger.Error(ex);
          }
          finally
          {
            lock (this._activeProcesses)
            {
              this._activeProcesses.Remove(process);
            }
            this._processSemaphore.Release();
          }
        });

        return process.StandardOutput.BaseStream;
      }
      catch (Exception ex)
      {
        this._logger.Error(ex);
        this._processSemaphore.Release();
        throw;
      }
    }

    public async Task<Boolean> PreTranscodeVideo(String videoPath, String outputPath, String quality = "medium")
    {
      if (!File.Exists(videoPath))
      {
        this._logger.ErrorFormat("Source file '{0}' does not exist.", videoPath);
        return false;
      }

      try
      {
        var outputDir = Path.GetDirectoryName(outputPath);
        if (!String.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
          Directory.CreateDirectory(outputDir);
        }

        this._logger.InfoFormat("Pre-transcoding '{0}' to '{1}' with quality '{2}'", videoPath, outputPath, quality);

        var args = this.BuildTranscodeArgs(videoPath, quality, outputPath: outputPath);

        var process = new Process
        {
          StartInfo = new ProcessStartInfo
          {
            FileName = "ffmpeg",
            Arguments = args,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true
          }
        };

        process.Start();
        await Task.Run(() => process.WaitForExit());

        var success = process.ExitCode == 0 && File.Exists(outputPath);

        if (success)
        {
          this._logger.InfoFormat("Successfully pre-transcoded '{0}' to '{1}'", videoPath, outputPath);
        }
        else
        {
          this._logger.ErrorFormat("Pre-transcoding failed for '{0}'. Exit code: {1}", videoPath, process.ExitCode);
        }

        return success;
      }
      catch (Exception ex)
      {
        this._logger.ErrorFormat("Pre-transcoding failed for '{0}': {1}", videoPath, ex.Message);
        this._logger.Error(ex);
        return false;
      }
    }

    public async Task<Int64> GetEstimatedSize(String videoPath, String quality = "medium")
    {
      try
      {
        var fileInfo = new FileInfo(videoPath);
        var compressionRatio = quality switch
        {
          "low" => 0.3,
          "medium" => 0.5,
          "high" => 0.7,
          "ultra" => 0.9,
          _ => 0.5
        };

        return (Int64)(fileInfo.Length * compressionRatio);
      }
      catch (Exception ex)
      {
        this._logger.Error(ex);
        return 0;
      }
    }

    public Boolean IsHardwareAccelerationAvailable()
    {
      if (this._hardwareAccelerationAvailable.HasValue)
        return this._hardwareAccelerationAvailable.Value;

      try
      {
        this._logger.DebugFormat("Checking hardware acceleration availability");

        var process = new Process
        {
          StartInfo = new ProcessStartInfo
          {
            FileName = "ffmpeg",
            Arguments = "-encoders",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
          }
        };

        process.Start();
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        this._hardwareAccelerationAvailable = output.Contains("h264_nvenc") || output.Contains("hevc_nvenc");

        this._logger.InfoFormat("Hardware acceleration available: {0}", this._hardwareAccelerationAvailable.Value);

        return this._hardwareAccelerationAvailable.Value;
      }
      catch (Exception ex)
      {
        this._logger.Error(ex);
        this._hardwareAccelerationAvailable = false;
        return false;
      }
    }

    public void CancelAllActiveStreams()
    {
      this._logger.InfoFormat("Cancelling all active streams. Active processes: {0}", this._activeProcesses.Count);

      lock (this._activeProcesses)
      {
        foreach (var process in this._activeProcesses.Where(p => !p.HasExited))
        {
          try
          {
            process.Kill();
            this._logger.DebugFormat("Killed process with ID: {0}", process.Id);
          }
          catch (Exception ex)
          {
            this._logger.WarnFormat("Failed to kill process: {0}", ex.Message);
          }
        }
        this._activeProcesses.Clear();
      }
    }

    private Process CreateTranscodingProcess(String videoPath, Double seekTime, Double playbackSpeed, String quality)
    {
      var args = this.BuildTranscodeArgs(videoPath, quality, seekTime, playbackSpeed);

      return new Process
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "ffmpeg",
          Arguments = args,
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          UseShellExecute = false,
          CreateNoWindow = true
        }
      };
    }

    private Process CreateSegmentProcess(String videoPath, Double startTime, Double duration, Double playbackSpeed, String quality)
    {
      var args = this.BuildTranscodeArgs(videoPath, quality, startTime, playbackSpeed, duration);

      return new Process
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "ffmpeg",
          Arguments = args,
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          UseShellExecute = false,
          CreateNoWindow = true
        }
      };
    }

    private String BuildTranscodeArgs(String videoPath, String quality, Double seekTime = 0, Double playbackSpeed = 1.0, Double? duration = null, String? outputPath = null)
    {
      var args = new StringBuilder();

      // Hardware acceleration setup
      if (this.IsHardwareAccelerationAvailable())
      {
        args.Append("-hwaccel nvdec -hwaccel_output_format cuda ");
      }

      // Seek to position (input seeking is more efficient)
      if (seekTime > 0)
      {
        args.AppendFormat("-ss {0:F3} ", seekTime);
      }

      // Input file
      args.AppendFormat("-i \"{0}\" ", videoPath);

      // Duration limit
      if (duration.HasValue)
      {
        args.AppendFormat("-t {0:F3} ", duration.Value);
      }

      // Video encoding settings
      if (this.IsHardwareAccelerationAvailable())
      {
        args.Append("-c:v h264_nvenc ");
        var nvencQuality = quality switch
        {
          "low" => "-preset fast -cq 28",
          "medium" => "-preset medium -cq 23",
          "high" => "-preset slow -cq 18",
          "ultra" => "-preset hq -cq 15",
          _ => "-preset medium -cq 23"
        };
        args.AppendFormat("{0} ", nvencQuality);
      }
      else
      {
        args.Append("-c:v libx264 ");
        if (this._qualityPresets.ContainsKey(quality))
        {
          args.AppendFormat("{0} ", this._qualityPresets[quality]);
        }
      }

      // Audio encoding
      args.Append("-c:a aac -b:a 128k ");

      // Playback speed adjustment
      if (Math.Abs(playbackSpeed - 1.0) > 0.01)
      {
        var filterValue = 1.0 / playbackSpeed;
        args.AppendFormat("-filter:v \"setpts={0:F3}*PTS\" ", filterValue);
        args.AppendFormat("-filter:a \"atempo={0:F3}\" ", playbackSpeed);
      }

      // Output format
      if (String.IsNullOrEmpty(outputPath))
      {
        // Streaming output
        args.Append("-f mp4 -movflags frag_keyframe+empty_moov+faststart pipe:1");
      }
      else
      {
        // File output
        args.AppendFormat("-f mp4 -movflags faststart \"{0}\"", outputPath);
      }

      return args.ToString();
    }
  }
}