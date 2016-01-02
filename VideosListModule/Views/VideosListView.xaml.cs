using System.Windows.Controls;
using VideoPlayer.Infrastructure;

namespace VideosListModule
{
    /// <summary>
    ///     Interaction logic for VideosListView.xaml
    /// </summary>
    public partial class VideosListView : UserControl, IVideosListView
    {
        public VideosListView()
        {
            this.InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel) this.DataContext; }
            set { this.DataContext = value; }
        }
    }
}