namespace VideoPlayer.Infrastructure
{
    public interface IView
    {
        IViewModel ViewModel { get; set; }
    }
}