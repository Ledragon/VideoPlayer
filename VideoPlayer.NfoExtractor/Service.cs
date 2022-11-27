using LeDragon.Log.Standard;
using VideoPlayer.Database.Repository;
using VideoPlayer.Entities;
using VideoPlayer.Nfo;

namespace VideoPlayer.NfoExtractor
{
    internal class Service
    {
        private readonly ILogger _logger;

        public Service()
        {
            this._logger = this.Logger();
        }

        public void GenerateNfo(String filePath, String sourceDir, String targetDir)
        {
            var repo = new FileVideoRepository();
            var ofw = repo.Load(filePath);
            var videos = ofw.Videos;
            var nfoSerializer = new NfoSerializer();
            var nfoService = new NfoService(nfoSerializer);
            videos.ForEach(v =>
            {
                try
                {
                    this._logger.DebugFormat("Creating nfo file for '{0}'.", v.FileName);
                    // TODO save preview in some cache folder
                    ReplaceDirectory(sourceDir, targetDir, v);
                    nfoService.CreateNfo(v);
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
