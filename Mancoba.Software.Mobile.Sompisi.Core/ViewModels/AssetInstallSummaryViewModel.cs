using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bluescore.DStv.Utils.Helpers.UserInteraction;
using Bluescore.DStv.Utils.Language;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Bluescore.DStv.Core.ViewModels
{
	public class AssetInstallSummaryViewModel :  MessengerBaseViewModel 
	{
		#region Constructor

		public AssetInstallSummaryViewModel(IMvxMessenger messenger)
			: base(messenger)
		{
		}



		#endregion

		#region Properties

		private List<AssetItemViewModel> _installItems;
		public List<AssetItemViewModel> InstallItems {
			get {
				return _installItems;
			}
			set {
				_installItems = value;
				RaisePropertyChanged (() => InstallItems);
			}
		}

		public string AssetSummary
		{
			get { return LanguageResolver.AssetSummary.ToUpperInvariant(); }
		}

		public string InstallSummary
		{
			get { return LanguageResolver.InstallSummary.ToUpperInvariant(); }
		}

		public string ConfigDetails
		{
			get { return LanguageResolver.ConfigDetails.ToUpperInvariant(); }
		}

		public string AssetIdLabel
		{
			get { return LanguageResolver.AssetId; }
		}

		private string _assetId;
		public string AssetId
		{
			get { return _assetId; }
			set
			{
				_assetId = value;
				RaisePropertyChanged(() => AssetId);
			}
		}

		public string MobileDeviceTypeLabel
		{
			get { return LanguageResolver.MobileDeviceType; }
		}

		private string _mobileDeviceType;
		public string MobileDeviceType
		{
			get { return _mobileDeviceType; }
			set
			{
				_mobileDeviceType = value;
				RaisePropertyChanged(() => MobileDeviceType);
			}
		}

		public string DeviceSerialLabel
		{
			get { return LanguageResolver.DeviceSerial; }
		}

		private string _deviceSerial;
		public string DeviceSerial
		{
			get { return _deviceSerial; }
			set
			{
				_deviceSerial = value;
				RaisePropertyChanged(() => DeviceSerial);
			}
		}

		public string ConfigGroupLabel
		{
			get { return LanguageResolver.ConfigurationGroup; }
		}

		private string _configGroup;
		public string ConfigGroup
		{
			get { return _configGroup; }
			set
			{
				_configGroup = value;
				RaisePropertyChanged(() => ConfigGroup);
			}
		}

		public string ConfigStatusLabel
		{
			get { return LanguageResolver.ConfigurationStatus; }
		}

		private string _configStatus;
		public string ConfigStatus
		{
			get { return _configStatus; }
			set
			{
				_configStatus = value;
				RaisePropertyChanged(() => ConfigStatus);
			}
		}

		public string DriverManagementLabel
		{
			get { return LanguageResolver.DriverManagement; }
		}

		private string _driverManagement;
		public string DriverManagement
		{
			get { return _driverManagement; }
			set
			{
				_driverManagement = value;
				RaisePropertyChanged(() => DriverManagement);
			}
		}

		public string StarterCutLabel
		{
			get { return LanguageResolver.StarterCut; }
		}

		private string _starterCut;
		public string StarterCut
		{
			get { return _starterCut; }
			set
			{
				_starterCut = value;
				RaisePropertyChanged(() => StarterCut);
			}
		}

		public string SpecialInstructionsLabel
		{
			get { return LanguageResolver.SpecialInstructions; }
		}

		private string _specialInstructions;
		public string SpecialInstructions
		{
			get { return _specialInstructions; }
			set
			{
				_specialInstructions = value;
				RaisePropertyChanged(() => SpecialInstructions);
			}
		}

		public string NextButtonLabel
		{
			get { return LanguageResolver.Done.ToUpperInvariant(); }
		}

		public string BackButtonLabel
		{
			get { return LanguageResolver.Back.ToUpperInvariant(); }
		}

		private IMvxAsyncCommand _nextCommand;
		public IMvxAsyncCommand NextCommand
		{
			get
			{
				_nextCommand = _nextCommand ?? new MvxAsyncCommand(DoNext);
				return _nextCommand;
			}
		}

		private IMvxAsyncCommand _backCommand;
		public IMvxAsyncCommand BackCommand
		{
			get
			{
				_backCommand = _backCommand ?? new MvxAsyncCommand(DoBack);
				return _backCommand;
			}
		}

		private IMvxAsyncCommand _specialInstructionsCommand;
		public IMvxAsyncCommand SpecialInstructionsCommand
		{
			get
			{
				_specialInstructionsCommand = _specialInstructionsCommand ?? new MvxAsyncCommand(DoSpecialInstructions);
				return _specialInstructionsCommand;
			}
		}

		#endregion

		#region Methods

		public void Init (CustomerItemViewModel customerItem)
		{
			
		}

		public void LoadInstallItems ()
		{
			_installItems = new List<AssetItemViewModel> ();

			var head = new AssetItemViewModel() {
				IsHeading = true,

				Wire = "Wire",
				Line = "Line",
				Connection = "Connection"

			};
			var item1 = new AssetItemViewModel() {
				IsHeading = false,
				Wire = "0",
				Line = "C1",
				Connection = "CAN: J1939 FMS Rev1.2.0.15",
				WireDrawable = "icon-wire-f1"

			};
			var item2 =
				new AssetItemViewModel () {
				IsHeading = false,
				Wire = "1",
				Line = "CP2",
				Connection = "Pico base Station",
				WireDrawable = "icon-wire-f2"

			};
			var item3 = new AssetItemViewModel () {
				IsHeading = false,
				Wire = "2",
				Line = "F1",
				Connection = "Speed Sender",
				WireDrawable = "icon-wire-f3"

			};
			_installItems.Add (head);
			_installItems.Add (item1);
			_installItems.Add (item2);
			_installItems.Add (item3);
			RaisePropertyChanged (() => InstallItems);
		}

		public async Task FetchAssetInstallSummary()
		{
			//Task.Run (async () => {

			IsLoading = true;
				try
				{
				await Task.Delay (2000);
					//var vm = await Mvx.Resolve<IMobileDataService>().FetchAssetInstallSummary();
				//	Mapper.Map(vm, this);
				}
				catch (Exception ex)
				{
					
				}
			//});

			//LoadInstallItems ();

			IsLoading = false;
		}

		private async Task DoBack()
		{
			Close (this);
			//ShowViewModel<CustomerListViewModel>();
		}

		private async Task DoNext()
		{
			Close (this);
		}

		private async Task DoSpecialInstructions()
		{
			if (string.IsNullOrEmpty (SpecialInstructions))
				return;
			Mvx.Resolve<IUserInteraction> ().Alert (SpecialInstructions, null, LanguageResolver.SpecialInstructions, LanguageResolver.Ok);
		}

		#endregion






	}
}
