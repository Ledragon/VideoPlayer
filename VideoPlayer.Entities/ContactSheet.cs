using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VideoPlayer.Entities
{
    public class ContactSheet
    {
        public Int32 Id { get; set; }
        [ForeignKey(nameof(Video))]
        public Int32 VideoId { get; set; }
        public Video Video { get; set; }
        public String Image { get; set; }
    }
}
