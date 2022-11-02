using Classes;
using VideoPlayer.Infrastructure.ViewFirst;
using VideosListModule.ViewModels;

namespace ManageLibraryModule
{
    public interface IEditViewModel : IViewModel
    {
        VideosCollectionView Videos { get; }
        VideoViewModel SelectedVideo { get; set; }
    }
}