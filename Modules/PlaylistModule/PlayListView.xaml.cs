using System.Windows.Controls;
using VideoPlayer.Infrastructure;

namespace PlaylistModule
{
    /// <summary>
    ///     Interaction logic for PlayListView.xaml
    /// </summary>
    public partial class PlayListView : UserControl, IPlayListView
    {
        public PlayListView(IPlayListViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
        }

        public IPlayListViewModel ViewModel
        {
            get { return (IPlayListViewModel) this.DataContext; }
            set { this.DataContext = value; }
        }
    }
}