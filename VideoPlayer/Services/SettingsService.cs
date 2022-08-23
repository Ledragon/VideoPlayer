using System;

namespace VideoPlayer.Services
{
    public class SettingsService : ISettingsService
    {
        public String VlcFolder => VideoPlayer.Properties.Settings.Default.VlcFolder;
    }
}
