using System.Collections.Generic;

namespace VideoPlayer.Ffmpeg
{
    public interface IFfmpegThumbnailGenerator
    {
        System.String GenerateContactSheet(System.String videoFilePath, System.Int32 rows, System.Int32 cols);
        List<System.String> GenerateThumbnails(System.String videoFilePath, System.Int32 count);
    }
}