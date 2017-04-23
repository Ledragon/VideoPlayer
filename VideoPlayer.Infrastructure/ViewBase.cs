using System.Windows.Controls;

namespace VideoPlayer.Infrastructure
{
    public class ViewBase : UserControl, IView
    {
        public ViewBase(IViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.ViewModel.View = this;
            this.DataContext = this.ViewModel;
        }

        public IViewModel ViewModel { get; set; }
    }
}