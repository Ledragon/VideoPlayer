using System.Windows.Controls;
using VideoPlayer.Infrastructure.ViewFirst;

namespace ManageLibraryModule
{
    /// <summary>
    ///     Interaction logic for ManagePageButtonView.xaml
    /// </summary>
    public partial class ManagePageButtonView : IManagePageButtonView
    {
        public ManagePageButtonView(IManagePageButtonViewModel viewModel)
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