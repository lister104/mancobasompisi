using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.Messages
{
	public class NavigateToMessage : MvxMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigateToMessage" /> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public NavigateToMessage(object sender)
                    : base(sender)
        {

        }
    }
}
