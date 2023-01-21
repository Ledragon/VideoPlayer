using VideoPlayer.Database.Repository;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Ffmpeg;
using VideoPlayer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddTransient<IVideoRepository, FileVideoRepository>()
    .AddSingleton<IFfprobeInfoExtractor,FfprobeInfoExtractor>()
    .AddSingleton<IFfmpegThumbnailGenerator, FfmpegThumbnailGenerator>()
    .AddSingleton<IPathService, PathService>();
//services cors
builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyHeader())
);

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

app.Run();
