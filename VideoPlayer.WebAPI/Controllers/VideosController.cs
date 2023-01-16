using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using VideoPlayer.Database.Repository;
using VideoPlayer.Entities;
using VideoPlayer.Services;

namespace VideoPlayer.WebAPI.Controllers
{
    [ApiController]
    [Route("api/videos")]
    public class VideosController : ControllerBase
    {
        private readonly ILogger<VideosController> _logger;
        private readonly IVideoRepository _videoRepository;
        private readonly IPathService _pathService;

        public VideosController(ILogger<VideosController> logger,
            IVideoRepository videoRepository,
            IPathService pathService)
        {
            this._logger = logger;
            this._videoRepository = videoRepository;
            this._pathService = pathService;
        }

        [HttpGet]
        public List<Video> GetVideos()
        {
            var filePath = this._pathService.GetLibraryFile();
            var videos = this._videoRepository.Load(filePath).Videos;
            return videos;
        }

        [HttpGet("/api/videos/metadata")]
        public Dictionary<String, VideoMetaData> GetMetaData()
        {
            var filePath = this._pathService.GetLibraryFile();
            var videos = this._videoRepository.Load(filePath).Videos;
            return videos.ToDictionary(d => d.FileName, d => new VideoMetaData { Codec = "", HasContactSheet = System.IO.File.Exists(d.FileName + ".png") });
        }

        [HttpGet("/api/videos/play")]
        public async  Task<FileStreamResult> PlayVideo()
        {
            var path = @"F:\Enfants\Disney\Aladdin 1.avi";
            var memory = new MemoryStream();

            using (var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await file.CopyToAsync(memory);
            }

            memory.Position = 0;
            var ext = Path.GetExtension(path).ToLowerInvariant();

            return File(memory, "video/avi");
            //var httpResponce = new HttpResponseMessage
            //{
            //    Content = new PushStreamContent((stream, content, context) => this.WriteContentToStream(path, stream))
            //};
            //return httpResponce;
        }

        public async void WriteContentToStream(String filePath, Stream outputStream)
        {
            //path of file which we have to read//  
            //here set the size of buffer, you can set any size  
            var bufferSize = 4096;
            var buffer = new Byte[bufferSize];
            //here we re using FileStream to read file from server//  
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int totalSize = (int)fileStream.Length;
                /*here we are saying read bytes from file as long as total size of file 

                is greater then 0*/
                while (totalSize > 0)
                {
                    int count = totalSize > bufferSize ? bufferSize : totalSize;
                    //here we are reading the buffer from orginal file  
                    int sizeOfReadedBuffer = fileStream.Read(buffer, 0, count);
                    //here we are writing the readed buffer to output//  
                    await outputStream.WriteAsync(buffer, 0, sizeOfReadedBuffer);
                    //and finally after writing to output stream decrementing it to total size of file.  
                    totalSize -= sizeOfReadedBuffer;
                }
            }
        }
    }
}
