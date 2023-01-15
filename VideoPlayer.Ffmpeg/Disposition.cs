using System;

namespace VideoPlayer.Ffmpeg
{
    public class Disposition
    {
        public Int32 _default { get; set; }
        public Int32 dub { get; set; }
        public Int32 original { get; set; }
        public Int32 comment { get; set; }
        public Int32 lyrics { get; set; }
        public Int32 karaoke { get; set; }
        public Int32 forced { get; set; }
        public Int32 hearing_impaired { get; set; }
        public Int32 visual_impaired { get; set; }
        public Int32 clean_effects { get; set; }
        public Int32 attached_pic { get; set; }
        public Int32 timed_thumbnails { get; set; }
        public Int32 captions { get; set; }
        public Int32 descriptions { get; set; }
        public Int32 metadata { get; set; }
        public Int32 dependent { get; set; }
        public Int32 still_image { get; set; }
    }

}
