using System.Windows.Controls;
using VideoPlayer.Infrastructure;

namespace VideosListModule.Views
{
    /// <summary>
    /// Interaction logic for VideoInfo.xaml
    /// </summary>
    public partial class VideoInfo : UserControl, IVideoInfoView
    {
        public VideoInfo()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel)this.DataContext; }
            set { this.DataContext = value; }
        }
    }

    public interface IVideoInfoView:IView
    {
    }
}
