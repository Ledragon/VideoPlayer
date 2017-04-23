using VideoPlayer.Infrastructure.ViewFirst;

namespace HomeModule
{
    public class VideoPlayer : IVideoPlayer
    {
        public VideoPlayer(IVideoPlayerViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }
        public IViewModel ViewModel { get; set; }
    }
}