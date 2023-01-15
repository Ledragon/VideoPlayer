namespace VideoPlayer.Ffmpeg
{
    public interface IFfprobeInfoExtractor
    {
        FfprobeVideoInfo GetVideoInfo(System.String filePath);
    }
}