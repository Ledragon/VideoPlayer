using Classes;
using VideoPlayer.Infrastructure;

namespace VideosListModule
{
    public interface IVideoInfoViewModel : IViewModel
    {
        Video Video { get; set; }
    }
}