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
            var cacheDir = GetCacheDirectory(targetDir);
            videos.ForEach(v =>
            {
                try
                {
                    this._logger.DebugFormat("Creating nfo file for '{0}'.", v.FileName);
                    ReplaceDirectory(sourceDir, targetDir, v);
                    // TODO save preview in some cache folder
                    if (!String.IsNullOrEmpty(v.SerializedImage))
                    {
                        var cacheFile = Path.Combine(cacheDir, Guid.NewGuid() + ".png");
                        this._imageExtractionService.SaveImage(v.SerializedImage, cacheFile);
                    }
                    nfoService.CreateNfo(v);
                }
                catch (Exception e)
                {
                    this._logger.Error(e);
                }
            });
        }

        private static String GetCacheDirectory(String targetDir)
        {
            var cacheDir = Path.Combine(targetDir, "cache");
            if (!System.IO.Directory.Exists(cacheDir))
            {
                System.IO.Directory.CreateDirectory(cacheDir);
            }

            return cacheDir;
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
