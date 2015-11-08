using System.Windows.Controls;
using VideoPlayer.Infrastructure;

namespace VideoPlayer.Views
{
    /// <summary>
    ///     Interaction logic for VideosListInteractionView.xaml
    /// </summary>
    public partial class VideosListInteractionView : UserControl, IVideosListInteractionView
    {
        public VideosListInteractionView()
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