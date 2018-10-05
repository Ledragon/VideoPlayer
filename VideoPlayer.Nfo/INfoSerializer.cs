namespace VideoPlayer.Nfo
{
    public interface INfoSerializer
    {
        void Serialize(MovieNfo nfo, System.String path);
    }
}