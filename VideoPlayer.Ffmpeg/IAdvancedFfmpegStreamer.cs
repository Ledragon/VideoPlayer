using System;
using System.IO;
using System.Threading.Tasks;

namespace VideoPlayer.Ffmpeg
{
  public interface IAdvancedFfmpegStreamer : IFfmpegStreamer
  {
    Task<System.IO.Stream?> StreamWithSeek(String videoPath, Double seekTimeSeconds, Double playbackSpeed = 1.0, String quality = "medium");
    Task<System.IO.Stream?> StreamSegment(String videoPath, Double startTime, Double duration, Double playbackSpeed = 1.0, String quality = "medium");
    Task<Boolean> PreTranscodeVideo(String videoPath, String outputPath, String quality = "medium");
    Task<Int64> GetEstimatedSize(String videoPath, String quality = "medium");
    Boolean IsHardwareAccelerationAvailable();
    void CancelAllActiveStreams();
  }
}