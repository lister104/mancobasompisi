using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace Bluescore.DStv.Core.ViewModels
{
	public class MyWorkOrderViewModel : MessengerBaseViewModel
	{
		#region Constructor

		public MyWorkOrderViewModel (IMvxMessenger messenger)
			: base(messenger)
		{
			

		}

		#endregion

		public void Init (CustomerItemViewModel customerItem)
		{

		}


		#region Commands
		private IMvxAsyncCommand _cancelCommand;
		public IMvxAsyncCommand CancelCommand {
			get {
				_cancelCommand = _cancelCommand ?? new MvxAsyncCommand(DoCancel);
				return _cancelCommand;
			}
		}


		#endregion

		#region Private Methods


		private async Task DoCancel ()
		{
			Close (this);
		}

		 

		#endregion

	}
}

