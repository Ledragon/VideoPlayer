using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoPlayer.Entities
{
    public class Thumbnail
    {
        public Int32 Id { get; set; }
        public String Image { get; set; }
        [ForeignKey(nameof(Video))]
        public Int32 VideoId { get; set; }
        public Video Video { get; set; }
    }
}
