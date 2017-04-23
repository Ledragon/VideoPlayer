using Module.Interfaces;
using VideoPlayer.Infrastructure;

namespace Module
{
    public class SettingsViewModel : ISettingsViewModel
    {
        public IView View { get; set; }
    }
}