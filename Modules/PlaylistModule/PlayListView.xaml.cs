using System.Windows.Controls;
using VideoPlayer.Infrastructure;

namespace PlaylistModule
{
    /// <summary>
    ///     Interaction logic for PlayListView.xaml
    /// </summary>
    public partial class PlayListView : UserControl, IPlayListView
    {
        public PlayListView()
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