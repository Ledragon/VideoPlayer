using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VideoPlayer.Entities
{
    public class TagVideo
    {
        public Int32 VideoId { get; set; }
        public Video Video { get; set; }
        public Int32 TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
