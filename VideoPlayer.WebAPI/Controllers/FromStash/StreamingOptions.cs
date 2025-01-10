namespace VideoPlayer.Streaming.Configuration;

public class StreamingOptions
{
    public string FFmpegPath { get; set; } = "ffmpeg";
    public int MaxTranscodeSize { get; set; } = 1920;
    public string TempPath { get; set; } = Path.GetTempPath();
}