using VideoPlayer.Infrastructure.ViewFirst;

namespace ManageLibraryModule
{
    /// <summary>
    ///     Interaction logic for ManageLibraryView.xaml
    /// </summary>
    public partial class ManageLibraryView : IManageLibraryView
    {
        public ManageLibraryView(IEditViewModel viewModel)
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