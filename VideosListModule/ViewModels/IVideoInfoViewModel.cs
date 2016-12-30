using System;
using Classes;
using VideoPlayer.Infrastructure;

namespace VideosListModule
{
    public interface IVideoInfoViewModel : IViewModel
    {
        Video Video { get; set; }
        String ContactSheetPath { get; }
        Boolean HasContactSheet { get; }
    }
}