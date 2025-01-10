namespace VideoPlayer.Streaming.Models;

public class VideoFile
{
    public int Id { get; init; }
    public required string Path { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
    public required VideoCodec VideoCodec { get; init; }
    public bool IsPrimary { get; init; }
}
public static class VideoFileExtensions
{
  public static VideoFile? GetPrimary(this ICollection<VideoFile> files) =>
      files.FirstOrDefault(f => f.IsPrimary);
}