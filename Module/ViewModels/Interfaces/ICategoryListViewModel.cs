using System.Collections.ObjectModel;
using System.Windows.Input;
using VideoPlayer.Infrastructure;

namespace Module.Interfaces
{
    public interface ICategoryListViewModel : IViewModel
    {
        ICommand FilterByCategoryCommand { get; set; }
        ObservableCollection<CategoryViewModel> CategoryViewModels { get; set; }
        CategoryViewModel SelectedCategory { get; set; }
    }
}