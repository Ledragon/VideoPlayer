using System;
using System.Collections.Generic;

namespace Classes
{
    public class Playlist
    {
        public String Title { get; set; }
        public DateTime CreationDate { get; set; }
        public List<String> Files { get; set; }

        public Playlist()
        {
            this.Files=new List<String>();
            this.CreationDate = DateTime.Now;
        }
    }
}