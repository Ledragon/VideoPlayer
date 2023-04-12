using LeDragon.Log.Standard;
using Microsoft.EntityFrameworkCore;
using VideoPlayer.Database.Repository;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Database.Repository.SQLite;
using VideoPlayer.Ffmpeg;
using VideoPlayer.Helpers;
using VideoPlayer.Services;
using VideoPlayer.WebAPI;

var builder = WebApplication.CreateBuilder(args);
LoggingSystemManager.SetPath(Path.Combine("./", "VideoPlayer.WebAPI.log"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterDependencies();

//services cors
builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*")
        .AllowAnyHeader()
        .AllowAnyMethod())
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

app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.Run();
