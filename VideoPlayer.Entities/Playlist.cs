using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace VideoPlayer.Entities
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
}