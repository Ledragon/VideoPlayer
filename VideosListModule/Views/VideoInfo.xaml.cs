using System.Windows.Controls;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideosListModule.Views
{
    /// <summary>
    ///     Interaction logic for VideoInfo.xaml
    /// </summary>
    public partial class VideoInfo : UserControl, IVideoInfoView
    {
        public VideoInfo(IVideoInfoViewModel viewModel)
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