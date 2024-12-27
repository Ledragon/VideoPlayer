using System;
using System.Threading.Tasks;

namespace VideoPlayer.Ffmpeg
{
  public interface IFfmpegStreamer
  {
    Task<System.IO.Stream?> Stream(String videoPath, Int64 startByte);
  }

}