using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.Messages
{
	public class NavigationBarHiddenMessage : MvxMessage
	{
		public bool NavigationBarHidden;

		public NavigationBarHiddenMessage(object sender, bool navigationBarHidden)
			: base(sender)
		{
			NavigationBarHidden = navigationBarHidden;
		}
	}
}

