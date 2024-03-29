﻿using LeDragon.Log.Standard;
using VideoPlayer.Database.Repository;
using VideoPlayer.ImageExtractor;
using VideoPlayer.Nfo;
using VideoPlayer.Services;

namespace VideoPlayer.NfoExtractor
{
    internal class Service
    {
        private readonly ILogger _logger;
        private readonly IImageExtractionService _imageExtractionService;
        private readonly PathReplacer _pathReplacer;

        public Service()
        {
            this._logger = this.Logger();
            this._imageExtractionService = new ImageExtractionService();
            this._pathReplacer = new PathReplacer();
        }

        public void GenerateNfo(String filePath, String sourceDir, String targetDir)
        {
            var repo = new FileLibraryRepository();
            var ofw = repo.Load(filePath);
            var videos = ofw.Videos;
            var nfoSerializer = new NfoSerializer();
            var nfoService = new NfoService(nfoSerializer);
            var cacheDirectory = CacheDirectoryProvider.GetCacheDirectory(targetDir);
            videos.ForEach(v =>
            {
                try
                {
                    this._logger.DebugFormat("Creating nfo file for '{0}'.", v.FileName);
                    this._pathReplacer.ReplaceDirectory(sourceDir, targetDir, v);
                    if (!String.IsNullOrEmpty(v.SerializedImage))
                    {
                        var cacheFile = v.GetThumbPath(cacheDirectory);
                        this._imageExtractionService.SaveImage(v.SerializedImage, cacheFile);
                    }
                    nfoService.CreateNfo(v, cacheDirectory);
                }
                catch (Exception e)
                {
                    this._logger.Error(e);
                }
            });
        }


        
    }
}
