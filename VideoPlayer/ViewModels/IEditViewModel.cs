using System.Collections.ObjectModel;
using Classes;

namespace VideoPlayer.ViewModels
{
    public interface IEditViewModel
    {
        ObservableCollection<Video> Videos { get; }
        Video SelectedVideo { get; set; }
    }
}