using System.Collections.ObjectModel;
using Classes;
using VideoPlayer.Infrastructure;

namespace Module.Interfaces
{
    public interface IVideosListViewModel:IViewModel
    {
        Video CurrentVideo { get; set; }
        ObservableCollection<Video> VideoCollection { get; set; }
    }
}