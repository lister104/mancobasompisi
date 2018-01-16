using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.Messages
{
	public class ToggleFlyoutMenuMessage : MvxMessage
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleFlyoutMenuMessage"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public ToggleFlyoutMenuMessage(object sender)
			: base(sender)
		{
		}
	}
}

