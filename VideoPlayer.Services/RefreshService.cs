﻿using LeDragon.Log.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;
using VideoPlayer.Helpers;

namespace VideoPlayer.Services
{
    public class RefreshService : IRefreshService
    {
        private readonly IVideoRepository _videoRepository;
        private readonly ITagsRepository _tagsRepository;

        public RefreshService(IVideoRepository videoRepository, ITagsRepository tagsRepository)
        {
            this._videoRepository = videoRepository;
            this._tagsRepository = tagsRepository;
        }

        public List<Video> Load(Directory directory)
        {
            var newVideos = new List<Video>();
            var existing = this._videoRepository.Get()
                .Select(d => d.FileName)
                .ToList();
            var tags = this._tagsRepository.Get();
            var files = DirectoryHelper.GetVideoFiles(directory.DirectoryPath, directory.IsIncludeSubdirectories)
                            .Where(videoFile => !existing.Contains(videoFile))
                            .ToList();
            this.Logger().DebugFormat("'{0}' new files found.", files.Count());
            foreach (var videoFile in files)
            {
                var newVideo = new Video(videoFile)
                {
                    Directory = directory
                };
                directory.Videos.Add(newVideo);
                foreach (var t in tags.Where(t => newVideo.Title.ToLower().Contains(t.Value)))
                {
                    newVideo.Tags.Add(t);
                    t.Videos.Add(newVideo);
                }
                newVideo.DateAdded = DateTime.Now;
                newVideos.Add(newVideo);
                this.Logger().DebugFormat("File '{0}' added.", newVideo.FileName);
            }
            if (newVideos.Any())
            {
                this._videoRepository.Add(newVideos);
            }
            return newVideos;
        }
        
        public List<Video> Clean(Directory directory)
        {
            var existing = this._videoRepository.Get()
                .Where(v => v.DirectoryId == directory.Id)
                .ToList();
            var tags = this._tagsRepository.Get();
            var files = DirectoryHelper.GetVideoFiles(directory.DirectoryPath, directory.IsIncludeSubdirectories)
                            .ToList();
            var notFound = existing.Where(d=>!files.Contains(d.FileName)).ToList();
            
            if (notFound.Any())
            {
                this._videoRepository.Delete(notFound);
            }
            return notFound;
        }
    }
}
