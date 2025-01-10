public interface IStreamTranscoder
{
    Task<RequestDelegate> CreateTranscodeHandlerAsync(
        TranscodeOptions options);
}