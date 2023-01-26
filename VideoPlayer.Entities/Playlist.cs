using System;
using System.Collections.Generic;

namespace VideoPlayer.Entities
{
    public class Playlist
    {
        public Playlist()
        {
            this.Items = new List<PlayListItem>();
            this.CreationDate = DateTime.Now;
        }

        public Int32 Id { get; set; }
        public String Title { get; set; }
        public DateTime CreationDate { get; set; }

        public List<PlayListItem> Items { get; set; }
    }
}