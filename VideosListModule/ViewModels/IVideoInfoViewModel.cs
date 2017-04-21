using System;
using Classes;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideosListModule
{
    public interface IVideoInfoViewModel : IViewModel
    {
        Video Video { get; set; }
        String ContactSheetPath { get; }
        Boolean HasContactSheet { get; }
    }
}