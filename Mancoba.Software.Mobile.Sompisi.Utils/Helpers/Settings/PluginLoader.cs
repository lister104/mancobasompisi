using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace Mancoba.Sompisi.Utils.Helpers.Settings
{	  	 
	public class PluginLoader : IMvxPluginLoader
	{
		public static readonly PluginLoader Instance = new PluginLoader();
        /// <summary>
        /// Ensures the loaded.
        /// </summary>
        public void EnsureLoaded()
		{
			var manager = Mvx.Resolve<IMvxPluginManager>();
			manager.EnsurePlatformAdaptionLoaded<PluginLoader>();
		}
	}
		
}

