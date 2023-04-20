using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Database.Repository.SQLite;
using VideoPlayer.Database.Repository;
using VideoPlayer.Ffmpeg;
using VideoPlayer.Helpers;
using VideoPlayer.Services;
using VideoPlayer.Database.UnitOfWork;

namespace VideoPlayer.WebAPI
{
    public static class DIExtension
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            return services.AddTransient<ILibraryRepository, FileLibraryRepository>()
                            .AddSingleton<IFfprobeInfoExtractor, FfprobeInfoExtractor>()
                            .AddSingleton<IFfmpegThumbnailGenerator, FfmpegThumbnailGenerator>()
                            .AddSingleton<IPathService, PathService>()
                            .AddTransient<IRefreshService, RefreshService>()
                            .AddTransient<IDirectoryRepository, SqliteDirectoryRepository>()
                            .AddTransient<IThumbnailsRepository, SqliteThumbnailsRepository>()
                            .AddTransient<IVideoRepository, SqliteVideoRepository>()
                            .AddTransient<ITagsRepository, SqliteTagsRepository>()
                            .AddTransient<ITagVideoRepository, SqliteTagVideoRepository>()
                            .AddTransient<IContactSheetsRepository, SqliteContactSheetsRepository>()
                            .AddTransient<ITagVideoUnitOfWork, TagVideoUnitOfWork>();
        }
    }
}
