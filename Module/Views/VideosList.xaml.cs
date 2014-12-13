using System.Windows.Controls;
using VideoPlayer.Infrastructure;

namespace Module
{
    /// <summary>
    ///     Interaction logic for VideosList.xaml
    /// </summary>
    public partial class VideosList : UserControl, IVideosList
    {
        public VideosList()
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