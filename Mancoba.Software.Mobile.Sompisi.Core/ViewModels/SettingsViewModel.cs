using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.ViewModels
{
	public class SettingsViewModel : MessengerBaseViewModel
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="messenger">The messenger.</param>
        public SettingsViewModel(IMvxMessenger messenger) : base(messenger)
		{
		}
	}
}

