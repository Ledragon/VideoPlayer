using System;
using System.ComponentModel.DataAnnotations.Schema;

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
        public Int32 Id { get; set; }

        [NotMapped]
        public String FileName { get; set; }
        
        [ForeignKey(nameof(Video))]
        public Int32 VideoId { get; set; }
        public Video Video { get; set; }
        public Int32 Order { get; set; }
        [ForeignKey(nameof(Playlist))]
        public Int32 PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}