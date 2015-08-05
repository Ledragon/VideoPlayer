using VideoPlayer.Infrastructure;

namespace HomeModule
{
    public class VideoPlayer:IVideoPlayer
    {
        public IViewModel ViewModel { get; set; }
    }
}