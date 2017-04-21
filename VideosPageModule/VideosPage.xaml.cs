using System.Windows.Controls;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideosPageModule
{
    /// <summary>
    ///     Interaction logic for VideosPage.xaml
    /// </summary>
    public partial class VideosPage : UserControl, IVideosPageView
    {
        public VideosPage(IVideosPageViewModel videosPageViewModel)
        {
            this.InitializeComponent();
            this.ViewModel = videosPageViewModel;
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel) this.DataContext; }
            set { this.DataContext = value; }
        }
    }
}