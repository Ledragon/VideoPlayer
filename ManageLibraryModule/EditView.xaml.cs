using VideoPlayer.Infrastructure.ViewFirst;

namespace ManageLibraryModule
{
    /// <summary>
    ///     Interaction logic for EditView.xaml
    /// </summary>
    public partial class EditView : IEditView
    {
        public EditView(IEditViewModel viewModel)
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