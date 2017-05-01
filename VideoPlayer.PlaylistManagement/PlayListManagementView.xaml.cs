using VideoPlayer.Infrastructure.ViewFirst;

namespace VideoPlayer.PlaylistManagement
{
    /// <summary>
    ///     Interaction logic for PlayListManagementView.xaml
    /// </summary>
    public partial class PlayListManagementView : IPlayListManagementView
    {
        public PlayListManagementView(IPlayListManagementViewModel playListManagementViewModel)
        {
            this.InitializeComponent();
            this.ViewModel = playListManagementViewModel;
        }


        public IViewModel ViewModel
        {
            get { return (IViewModel) this.DataContext; }
            set { this.DataContext = value; }
        }
    }
}