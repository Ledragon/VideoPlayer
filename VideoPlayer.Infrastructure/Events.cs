using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Documents;
using Classes;
using Microsoft.Practices.Prism.PubSubEvents;

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

    public class VideoAddedEvent : PubSubEvent<Video>
    {
    }

    public class PlayPlaylistEvent : PubSubEvent<Object>
    {
    }

    public class PlayOneEvent : PubSubEvent<Video>
    {
    }

    public class PlayedEvent : PubSubEvent<Video>
    {
    }

    public class StoppedEvent : PubSubEvent<Object>
    {
    }

    public class RateChanged : PubSubEvent<Single>
    {
    }

    public class VideoDurationChanged : PubSubEvent<TimeSpan>
    {
    }

    public class VideoEnded : PubSubEvent<object>
    {
    }

    public class VideoPositionChanged : PubSubEvent<Single>
    {
    }

    public class VideoEditing : PubSubEvent<Video>
    {
        
    }
    public class VideoEdited : PubSubEvent<Object>
    {
    }

    public class LibraryUpdated : PubSubEvent<Object>
    {
    }

    public class PlayAllEvent : PubSubEvent<Object>
    {
    }

    public class ClearPlaylistEvent : PubSubEvent<Object>
    {
    }

    public class GoToPage : PubSubEvent<Int32>
    {
    }
}