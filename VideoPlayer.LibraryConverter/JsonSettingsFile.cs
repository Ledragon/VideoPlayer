namespace VideoPlayer.LibraryConverter
{
    public class JsonSettingsFile
    {
        public String SourceFile { get; set; }
        public String TargetFile { get; set; }
        public Dictionary<String, String> Remap { get; set; }
        public String CacheDirectory { get; set; }
    }
}
