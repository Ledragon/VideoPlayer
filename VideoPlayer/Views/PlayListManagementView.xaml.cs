using System.Windows.Controls;
using VideoPlayer.Infrastructure;

namespace VideoPlayer.Views
{
    /// <summary>
    ///     Interaction logic for PlayListManagementView.xaml
    /// </summary>
    public partial class PlayListManagementView : IPlayListManagementView
    {
        public PlayListManagementView()
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