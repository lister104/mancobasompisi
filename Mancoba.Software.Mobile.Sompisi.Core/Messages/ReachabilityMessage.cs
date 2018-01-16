using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.Messages
{
	public class ReachabilityMessage: MvxMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReachabilityMessage" /> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public ReachabilityMessage(object sender)
                    : base(sender)
        {

        }
    }
}

