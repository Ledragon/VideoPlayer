using System.Windows.Controls;
using VideoPlayer.Infrastructure.ViewFirst;

namespace Module
{
    /// <summary>
    ///     Interaction logic for EditVideo.xaml
    /// </summary>
    public partial class EditVideo : UserControl, IEditView
    {
        public EditVideo(IEditVideoViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
        }

        public IViewModel ViewModel
        {
            get { return this.DataContext as IViewModel; }
            set { this.DataContext = value; }
        }
    }
}