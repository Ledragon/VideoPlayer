using System.Windows.Controls;
using VideoPlayer.Common;
using VideoPlayer.Services;
using VideoPlayer.ViewModels;

namespace VideoPlayer.Views
{
    /// <summary>
    ///     Interaction logic for EditView.xaml
    /// </summary>
    public partial class EditView : UserControl
    {
        public EditView()
        {
            this.InitializeComponent();
            this.DataContext = DependencyFactory.Resolve<IEditViewModel>();
        }
    }
}