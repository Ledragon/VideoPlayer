
public interface IStreamManager
{
    Task ServeTranscodeAsync(HttpContext context, TranscodeOptions options);
    bool IsTranscodingEnabled { get; }
}