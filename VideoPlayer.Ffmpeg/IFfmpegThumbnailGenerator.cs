using System;
using System.Collections.Generic;

namespace VideoPlayer.Ffmpeg
{
  public interface IFfmpegThumbnailGenerator
  {
    String GenerateContactSheet(String videoFilePath, Int32 rows, Int32 cols);
    List<String> GenerateThumbnails(String videoFilePath, Int32 count, Int32 width = 640);
  }
}