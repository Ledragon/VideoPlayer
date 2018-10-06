using System.Windows.Controls;
using VideoPlayer.Infrastructure.ViewFirst;

namespace Module.Views
{
    /// <summary>
    ///     Interaction logic for SortGrid.xaml
    /// </summary>
    public partial class SortGrid : UserControl, ISortGrid
    {
        public SortGrid(ISortGridViewModel viewModel)
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

    public interface ISortGrid : IView
    {
    }
}