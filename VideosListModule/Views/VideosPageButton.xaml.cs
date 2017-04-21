using VideoPlayer.Infrastructure.ViewFirst;
using VideosListModule.ViewModels;

namespace VideosListModule.Views
{
    /// <summary>
    ///     Interaction logic for VideosPageButton.xaml
    /// </summary>
    public partial class VideosPageButton : IVideosPageButtonView
    {
        public VideosPageButton(IVideosPageButtonViewModel viewModel)
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