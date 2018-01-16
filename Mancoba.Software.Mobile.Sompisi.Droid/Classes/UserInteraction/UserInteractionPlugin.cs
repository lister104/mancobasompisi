using Mancoba.Sompisi.Utils.Helpers.UserInteraction;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace Mancoba.Sompisi.Droid.Classes.UserInteraction
{
    public class UserInteracionPlugin : IMvxPlugin
    {
        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            Mvx.RegisterType<IUserInteraction, UserInteraction>();
        }
    }
}