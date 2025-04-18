﻿using LeDragon.Log.Standard;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;
using VideoPlayer.Ffmpeg;
using VideoPlayer.Helpers;
using Directory = VideoPlayer.Entities.Directory;

namespace VideoPlayer.Services
{
  public class RefreshService : IRefreshService
  {
    private readonly IVideoRepository _videoRepository;
    private readonly ITagsRepository _tagsRepository;
    private readonly IFfprobeInfoExtractor _ffprobeInfoExtractor;
    private readonly IFfmpegThumbnailGenerator _ffmpegThumbnailGenerator;
    private readonly IThumbnailsRepository _thumbnailsRepository;
    private readonly ILogger _logger;

    public RefreshService(IVideoRepository videoRepository, ITagsRepository tagsRepository,
        IFfprobeInfoExtractor ffprobeInfoExtractor,
        IFfmpegThumbnailGenerator ffmpegThumbnailGenerator,
        IThumbnailsRepository thumbnailsRepository)
    {
      this._videoRepository = videoRepository;
      this._tagsRepository = tagsRepository;
      this._ffprobeInfoExtractor = ffprobeInfoExtractor;
      this._ffmpegThumbnailGenerator = ffmpegThumbnailGenerator;
      this._thumbnailsRepository = thumbnailsRepository;
      this._logger = this.Logger();
    }

    public List<Video> Load(Directory directory)
    {
      var newVideos = new List<Video>();
      var existing = this._videoRepository.Get().ToDictionary(d => d.FileName);
      var tags = this._tagsRepository.Get();
      var files = DirectoryHelper.GetVideoFiles(directory.DirectoryPath, directory.IsIncludeSubdirectories);
      var newFiles = files
                      .Where(videoFile => !existing.ContainsKey(videoFile))
                      .ToList();
      this._logger.DebugFormat("'{0}' new files found.", newFiles.Count());
      foreach (var videoFile in newFiles)
      {
        var newVideo = CreateVideo(directory, tags, videoFile);
        newVideo = this.SetInfo(newVideo);
        newVideos.Add(newVideo);
        this._logger.DebugFormat("File '{0}' added.", newVideo.FileName);
      }

      if (newVideos.Any())
      {
        this._videoRepository.Add(newVideos);
      }

      var toUpdate = this.GetVideosToUpdate(existing.Values.ToList())
          .Select(v => this.SetInfo(v))
          .ToList();
      if (toUpdate.Any())
      {
        this._videoRepository.Update(toUpdate);
        this._logger.InfoFormat("Updated {{0}} videos.");
      }
      return newVideos;
    }

    private static Video CreateVideo(Directory directory, List<Tag> tags, String videoFile)
    {
      var newVideo = new Video(videoFile)
      {
        Directory = directory,
        Thumbnails = new List<Thumbnail>()
      };
      if (directory.Videos != null)
      {
        directory.Videos.Add(newVideo);
      }
      foreach (var t in tags.Where(t => newVideo.Title.ToLower().Contains(t.Value.ToLower())))
      {
        newVideo.Tags.Add(t);
        if (t.Videos == null)
        {
          t.Videos = new List<Video>();
        }
        t.Videos.Add(newVideo);
      }
      newVideo.DateAdded = DateTime.Now;
      return newVideo;
    }

    private Video SetInfo(Video video)
    {
      if (File.Exists(video.FileName))
      {
        var info = this._ffprobeInfoExtractor.GetVideoInfo(video.FileName);
        if (video.Length == TimeSpan.Zero)
        {
          video.Length = TimeSpan.FromSeconds(Math.Round(Double.Parse(info.format.duration, CultureInfo.InvariantCulture)));
        }

        //video.Thumbnails = this._thumbnailsRepository.GetForVideo(video.Id);
        //if (!video.Thumbnails.Any())
        //{
        //    //var thumbs = this._ffmpegThumbnailGenerator.GenerateThumbnails(video.FileName, 1);
        //    //thumbs.ForEach(t =>
        //    //{
        //    //    var converted = ToBase64(t);
        //    //    video.Thumbnails.Add(new Thumbnail { Image = converted });
        //    //});
        //}

        //if (String.IsNullOrEmpty(video.ContactSheet))
        //{
        //    var file = File.Exists(video.FileName + ".png") ? video.FileName + ".png" : this._ffmpegThumbnailGenerator.GenerateContactSheet(video.FileName, 3, 4);
        //    video.ContactSheet = ToBase64(file);
        //}
      }
      return video;
    }


    private List<Video> GetVideosToUpdate(List<Video> existing)
    {
      return existing.Where(v => v.Length == TimeSpan.Zero
      || !this._thumbnailsRepository.GetForVideo(v.Id).Any()
      ).ToList();
    }

    public List<Video> Clean(Directory directory)
    {
      var existing = this._videoRepository.Get()
          .Where(v => v.DirectoryId == directory.Id)
          .ToList();
      var tags = this._tagsRepository.Get();
      var files = DirectoryHelper.GetVideoFiles(directory.DirectoryPath, directory.IsIncludeSubdirectories)
                      .ToList();
      var notFound = existing.Where(d => !files.Contains(d.FileName)).ToList();

      var duplicates = existing.GroupBy(v => v.FileName)
          .Where(g => g.Count() > 1)
          .SelectMany(g => g.Skip(1))
          .ToList();

      if (notFound.Concat(duplicates).Any())
      {
        this._videoRepository.Delete(notFound.Concat(duplicates).ToList());
      }
      return notFound;
    }
  }
}
