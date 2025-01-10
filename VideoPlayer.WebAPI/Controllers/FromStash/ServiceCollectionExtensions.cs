namespace VideoPlayer.Streaming.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStreamingServices(this IServiceCollection services)
    {
        services.AddSingleton<TranscodeContext>();
        services.AddScoped<IStreamTranscoder, StreamTranscoder>();
        services.AddScoped<IStreamManager, StreamManager>();
        
        return services;
    }
}