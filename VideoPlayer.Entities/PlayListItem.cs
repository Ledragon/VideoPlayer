using System;

namespace VideoPlayer.Entities
{
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