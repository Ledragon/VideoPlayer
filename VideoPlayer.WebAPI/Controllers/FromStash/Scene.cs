namespace VideoPlayer.Streaming.Models;

public class Scene
{
    public int Id { get; init; }
    public ICollection<VideoFile> Files { get; init; } = new List<VideoFile>();
}