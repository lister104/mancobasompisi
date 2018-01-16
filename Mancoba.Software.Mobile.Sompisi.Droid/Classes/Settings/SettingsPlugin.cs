using Mancoba.Sompisi.Utils.Helpers.Settings;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace Mancoba.Sompisi.Droid.Classes.Settings
{
    public class SettingsPlugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<ISettings>(new MvxAndroidSettings());
        }
    }
}

