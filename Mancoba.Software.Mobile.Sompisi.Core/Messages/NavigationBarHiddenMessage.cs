using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.Messages
{
	public class NavigationBarHiddenMessage : MvxMessage
	{
		public bool NavigationBarHidden;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationBarHiddenMessage"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="navigationBarHidden">if set to <c>true</c> [navigation bar hidden].</param>
        public NavigationBarHiddenMessage(object sender, bool navigationBarHidden)
			: base(sender)
		{
			NavigationBarHidden = navigationBarHidden;
		}
	}
}

