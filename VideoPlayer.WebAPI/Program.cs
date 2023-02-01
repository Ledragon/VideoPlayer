using Microsoft.EntityFrameworkCore;
using VideoPlayer.Database.Repository;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Database.Repository.SQLite;
using VideoPlayer.Ffmpeg;
using VideoPlayer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddTransient<ILibraryRepository, FileLibraryRepository>()
    .AddSingleton<IFfprobeInfoExtractor,FfprobeInfoExtractor>()
    .AddSingleton<IFfmpegThumbnailGenerator, FfmpegThumbnailGenerator>()
    .AddSingleton<IPathService, PathService>()
    .AddTransient<IThumbnailsRepository, SqliteThumbnailsRepository>()
    .AddTransient<IVideoRepository, SqliteVideoRepository>()
    ;
//services cors
builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyHeader())
);

builder.Services.AddDbContext<VideoPlayerContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Database")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseCors();
app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();
