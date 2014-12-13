using System.Collections.ObjectModel;
using Classes;
using VideoPlayer.Infrastructure;

namespace Module
{
    public interface IVideosListViewModel:IViewModel
    {
        Video CurrentVideo { get; set; }
        ObservableCollection<Video> VideoCollection { get; set; }
    }
}