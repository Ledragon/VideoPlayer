using System;
using Classes;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideosListModule
{
    public interface IVideoInfoViewModel : IViewModel
    {
        VideoViewModel Video { get; set; }
        String ContactSheetPath { get; }
        Boolean HasContactSheet { get; }
    }
}