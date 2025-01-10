namespace VideoPlayer.Streaming.Models;

public sealed class StreamFormat
{
    public required string MimeType { get; init; }
    public required Func<VideoCodec, VideoFilter, bool, IEnumerable<string>> Args { get; init; }

    public static StreamFormat Mkv => new()
    {
        MimeType = "video/x-matroska",
        Args = (codec, filter, videoOnly) =>
        {
            var args = new List<string>();
            args.AddRange(new[] { "-c:v", codec.ToString().ToLowerInvariant() });
            
            if (videoOnly)
            {
                args.Add("-an");
            }
            else
            {
                args.AddRange(new[]
                {
                    "-c:a", "libopus",
                    "-b:a", "96k",
                    "-vbr", "on",
                    "-ac", "2"
                });
            }
            
            args.AddRange(new[] { "-f", "matroska" });
            return args;
        }
    };

    public static StreamFormat Hls => new()
    {
        MimeType = "application/x-mpegURL",
        Args = (codec, filter, videoOnly) =>
        {
            var args = new List<string>();
            args.AddRange(new[]
            {
                "-c:v", codec.ToString().ToLowerInvariant(),
                "-hls_time", "6",
                "-hls_list_size", "0",
                "-hls_playlist_type", "event",
                "-hls_segment_type", "mpegts"
            });

            if (!videoOnly)
            {
                args.AddRange(new[]
                {
                    "-c:a", "aac",
                    "-b:a", "128k",
                    "-ac", "2"
                });
            }

            args.AddRange(new[] { "-f", "hls" });
            return args;
        }
    };

    public static StreamFormat Dash => new()
    {
        MimeType = "application/dash+xml",
        Args = (codec, filter, videoOnly) =>
        {
            var args = new List<string>();
            args.AddRange(new[]
            {
                "-c:v", codec.ToString().ToLowerInvariant(),
                "-use_timeline", "1",
                "-use_template", "1",
                "-seg_duration", "6",
                "-adaptation_sets", "id=0,streams=v id=1,streams=a"
            });

            if (!videoOnly)
            {
                args.AddRange(new[]
                {
                    "-c:a", "aac",
                    "-b:a", "128k",
                    "-ac", "2"
                });
            }

            args.AddRange(new[] { "-f", "dash" });
            return args;
        }
    };
}