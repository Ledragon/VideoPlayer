using System;
using Classes;
using Microsoft.Practices.Prism.PubSubEvents;

namespace VideoPlayer.Infrastructure
{
    public class SelectedCategoryChangedEvent : PubSubEvent<String>
    {
    }

    public class VideoAddedEvent : PubSubEvent<Video>
    {
    }

    public class PlayAllEvent : PubSubEvent<Object>
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
    
}