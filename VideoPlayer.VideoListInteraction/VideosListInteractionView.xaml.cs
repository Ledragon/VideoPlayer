using VideoPlayer.Infrastructure.ViewFirst;

namespace VideoPlayer.VideoListInteraction
{
    /// <summary>
    ///     Interaction logic for VideosListInteractionView.xaml
    /// </summary>
    public partial class VideosListInteractionView : IVideosListInteractionView
    {
        public VideosListInteractionView(IVideosListInteractionViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel) this.DataContext; }
            set { this.DataContext = value; }
        }
    }
}