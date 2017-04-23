using System.Windows.Controls;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideosListModule
{
    /// <summary>
    ///     Interaction logic for VideosListView.xaml
    /// </summary>
    public partial class VideosListView : UserControl, IVideosListView
    {
        public VideosListView(IVideosListViewModel viewModel)
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