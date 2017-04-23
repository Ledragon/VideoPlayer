using VideoPlayer.Infrastructure.ViewFirst;

namespace PlaylistModule
{
    /// <summary>
    ///     Interaction logic for PlayListView.xaml
    /// </summary>
    public partial class PlayListView : IPlayListView
    {
        public PlayListView(IPlayListViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel)this.DataContext; }
            set { this.DataContext = value; }
        }
    }
}