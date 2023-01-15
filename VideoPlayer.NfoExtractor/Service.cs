using LeDragon.Log.Standard;
using VideoPlayer.Database.Repository;
using VideoPlayer.Entities;
using VideoPlayer.ImageExtractor;
using VideoPlayer.Nfo;

namespace VideoPlayer.NfoExtractor
{
    internal class Service
    {
        private readonly ILogger _logger;
        private readonly IImageExtractionService _imageExtractionService;

        public Service()
        {
            this._logger = this.Logger();
            this._imageExtractionService = new ImageExtractionService();
        }

        public void GenerateNfo(String filePath, String sourceDir, String targetDir)
        {
            var repo = new FileVideoRepository();
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
                    ReplaceDirectory(sourceDir, targetDir, v);
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


        private static void ReplaceDirectory(String sourceDir, String targetDir, Video v)
        {
            if (!String.IsNullOrEmpty(sourceDir)
            && !String.IsNullOrEmpty(targetDir)
            && sourceDir != targetDir)
            {
                v.Directory = v.Directory
                    .Replace(sourceDir, targetDir)
                    .Replace("\\", "/");
                v.FileName = v.FileName
                    .Replace(sourceDir, targetDir)
                    .Replace("\\", "/");
            }
        }
    }
}
