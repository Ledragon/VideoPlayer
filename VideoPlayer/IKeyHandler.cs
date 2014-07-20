using System.Windows.Input;

namespace VideoPlayer
{
    public interface IKeyHandler
    {
        void HandleKey(KeyEventArgs e);
    }
}