using System;
using System.Collections.Generic;
using System.ComponentModel;
using Classes;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Entities;

namespace VideoPlayer.Infrastructure
{
    public class SelectedCategoryChangedEvent : PubSubEvent<String>
    {
    }

    public class NameFilterChangedEvent : PubSubEvent<String>
    {
    }

    public class TagFilterChangedEvent : PubSubEvent<List<String>>
    {
    }

    public class SortingChangedEvent : PubSubEvent<SortDescription>
    {
    }

    public class FilterChangedEvent : PubSubEvent<Int32>
    {
    }

    public class OnAddVideo : PubSubEvent<VideoViewModel>
    {
    }

    public class OnAddVideoRange : PubSubEvent<IEnumerable<VideoViewModel>>
    {
    }

    public class OnAddVideoRangeRequest : PubSubEvent<Object>
    {
    }

    public class OnPlayPlaylistRequest : PubSubEvent<Object>
    {
    }

    public class OnPlayPlaylist : PubSubEvent<IEnumerable<VideoViewModel>>
    {
    }

    public class PlayOneRequestEvent : PubSubEvent<VideoViewModel> { }
    public class PlayOneEvent : PubSubEvent<VideoViewModel>
    {
    }

    public class PlayedEvent : PubSubEvent<VideoViewModel>
    {
    }

    public class OnStop : PubSubEvent<Object>
    {
    }
    public class OnStopped : PubSubEvent<Object>
    {
    }

    public class RateChanged : PubSubEvent<Single>
    {
    }

    public class VideoDurationChanged : PubSubEvent<TimeSpan>
    {
    }

    public class VideoEnded : PubSubEvent<Object>
    {
    }

    public class VideoPositionChanged : PubSubEvent<Single>
    {
    }

    public class VideoEditing : PubSubEvent<VideoViewModel>
    {
    }


    public class SetVideo : PubSubEvent<VideoViewModel>
    {
    }

    public class VideoEdited : PubSubEvent<Object>
    {
    }

    public class LibraryUpdating : PubSubEvent<String>
    {
    }

    public class LibraryUpdated : PubSubEvent<IEnumerable<Video>>
    {
    }

    public class AddAllEvent : PubSubEvent<IEnumerable<VideoViewModel>>
    {
    }

    public class PlayAllEvent : PubSubEvent<Object>
    {
    }
    
    public class PlayRangeEvent : PubSubEvent<IEnumerable<VideoViewModel>>
    {
    }

    public class ClearPlaylistEvent : PubSubEvent<Object>
    {
    }

    public class GoToPage : PubSubEvent<Int32>
    {
    }

    public class CloseRequestedEvent : PubSubEvent<Object>
    {
    }

    public class VideoSelected : PubSubEvent<VideoViewModel> { }



    public class PlayCompleted : PubSubEvent<Object> 
    {
    }
}