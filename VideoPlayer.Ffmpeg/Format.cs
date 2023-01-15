using System;

namespace VideoPlayer.Ffmpeg
{
    public class Format
    {
        public String filename { get; set; }
        public Int32 nb_streams { get; set; }
        public Int32 nb_programs { get; set; }
        public String format_name { get; set; }
        public String format_long_name { get; set; }
        public String start_time { get; set; }
        public String duration { get; set; }
        public String size { get; set; }
        public String bit_rate { get; set; }
        public Int32 probe_score { get; set; }
    }
}
