using System.Collections.ObjectModel;
using VideoPlayer.Infrastructure.ViewFirst;

namespace Module
{
    public interface ITagsListViewModel : IViewModel
    {
        ObservableCollection<CategoryViewModel> Tags { get; set; }
        ObservableCollection<CategoryViewModel> SelectedTags { get; set; }
    }
}