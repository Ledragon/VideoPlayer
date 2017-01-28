using System.Collections.ObjectModel;
using Classes;
using VideosListModule.ViewModels;

namespace VideoPlayer.ViewModels
{
    public interface IEditViewModel
    {
        VideosCollectionView Videos { get; }
        Video SelectedVideo { get; set; }
    }
}