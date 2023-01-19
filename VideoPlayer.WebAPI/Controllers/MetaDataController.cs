using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Ffmpeg;

namespace VideoPlayer.WebAPI.Controllers
{
    [Route("api/metadata")]
    public class MetadataController : ControllerBase
    {
        public MetadataController()
        {
        }

        [HttpGet]
        public List<VideoMetaData> Metadata()
        {
            return new List<VideoMetaData>();
        }
        
        [HttpGet]
        public VideoMetaData Metadata([FromQuery] String filePath)
        {
            return new VideoMetaData();
        }

    }
}
