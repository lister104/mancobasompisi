using System.Linq;
using System.Threading.Tasks;
using Mancoba.Sompisi.Utils.Enums;
using Mancoba.Sompisi.Utils.Helpers.UserInteraction;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace Mancoba.Sompisi.Core.ViewModels
{
	public class MessengerBaseViewModel : MvxViewModel
	{
		protected IMvxMessenger Messenger;
		private bool _isLoading;
		private string _modelName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessengerBaseViewModel"/> class.
        /// </summary>
        /// <param name="messenger">The messenger.</param>
        public MessengerBaseViewModel(IMvxMessenger messenger)
		{
			Messenger = messenger;

			CrossConnectivity.Current.ConnectivityChanged += (sender, args) => {
				if (args.IsConnected) {
					string connectedTo = LanguageResolver.ViaWiFi;
					ConnectionType type = CrossConnectivity.Current.ConnectionTypes.FirstOrDefault();
                    switch (type)
                    {
                        case ConnectionType.WiFi:
                            connectedTo = LanguageResolver.ViaWiFi;
                            break;
                        case ConnectionType.Cellular:
                        case ConnectionType.Wimax:
                        default:
                            connectedTo = LanguageResolver.ViaData;
                            break;
                    }
				string message = string.Format (@"{0} {1}", LanguageResolver.InternetConnected, connectedTo);
					Mvx.Resolve<IUserInteraction> ().ToastAlert (message,ToastGravity.Bottom);
				}
				else
					Mvx.Resolve<IUserInteraction> ().ToastAlert (LanguageResolver.NoConnectivity,ToastGravity.Bottom);				 
			};
		}

		public string ModelName
		{
			get
			{
				return _modelName;
			}

			set
			{
				_modelName = value;
				RaisePropertyChanged(() => ModelName);
			}
		}

		public bool IsLoading
		{
			get { return _isLoading; }
			set
			{
				_isLoading = value;
				RaisePropertyChanged(() => IsLoading);
				RaisePropertyChanged(() => IsNotLoading);
			}
		}

		public bool IsNotLoading
		{
			get { return !IsLoading; }
		}

		private bool _hasFetched;

		public bool HasFetched {
			get { return _hasFetched; }
			set {
				_hasFetched = value;
				RaisePropertyChanged (() => HasFetched);				 
			}
		}

		public string BusyMessage { get; set; }

		private IMvxAsyncCommand _refreshCommand;

        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        /// <value>
        /// The refresh command.
        /// </value>
        public IMvxAsyncCommand RefreshCommand
		{
			get
			{
				return _refreshCommand = _refreshCommand ?? new MvxAsyncCommand(RefreshData);				
			}
		}

        /// <summary>
        /// Refreshes the data.
        /// </summary>
        /// <returns></returns>
        public virtual async Task RefreshData()
		{
		    await Task.Run(() =>
		    {
		        IsLoading = true;               
		        IsLoading = false;
		    });
		}
	}
}

