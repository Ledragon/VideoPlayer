using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Classes
{
    public class Playlist
    {
        public Playlist()
        {
            this.Items = new List<PlayListItem>();
            this.CreationDate = DateTime.Now;
        }

        public String Title { get; set; }
        public DateTime CreationDate { get; set; }
        
        public List<PlayListItem> Items { get; set; }
    }

    public class PlayListItem
    {
        public PlayListItem()
        {
            
        }
        public PlayListItem(String fileName, Int32 order)
        {
            this.FileName = fileName;
            this.Order = order;
        }

        public String FileName { get; set; }
        public Int32 Order { get; set; }
    }
}