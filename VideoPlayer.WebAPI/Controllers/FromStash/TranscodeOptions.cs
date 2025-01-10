
using VideoPlayer.Streaming.Models;

public class TranscodeOptions
{
    public required StreamFormat StreamType { get; init; }
    public required VideoFile VideoFile { get; init; }
    public string? Resolution { get; init; }
    public double StartTime { get; init; }
}