using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mancoba.Sompisi.Core.Services.Contracts;
using Mancoba.Sompisi.Data.Models;
using Mancoba.Sompisi.Utils.Enums;
using Mancoba.Sompisi.Utils.Helpers.UserInteraction;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.ViewModels
{
	public class InstallerListViewModel : MessengerBaseViewModel
	{
		#region Constructor

		public InstallerListViewModel(IMvxMessenger messenger)
			: base(messenger)
		{
			
		}

        public void Init(List<ModelInstaller> installers)
        {
            ShowList(installers);
        }

        #endregion

        #region Properties

        public string CancelLabel => LanguageResolver.Cancel.ToUpperInvariant();

		public string Count => string.Format(LanguageResolver.InstallerCountLabel, AllList.Count.ToString());
		
		private string _searchString;

		public string SearchString
		{
			get { return _searchString; }
			set
			{
				_searchString = value;

				if (string.IsNullOrEmpty(value))
				{

					FilteredList = AllList;
				}
				else
				{
					FilteredList = AllList.Where(o => o.Name.ToLowerInvariant().Contains(value.ToLowerInvariant()) ||
					                                    o.ContactPerson.ToLowerInvariant().Contains(value.ToLowerInvariant()) ||
					                                    o.PhoneNumber.ToLowerInvariant().Contains(value.ToLowerInvariant())).ToList();

				}

				RaisePropertyChanged(() => SearchString);
				RaisePropertyChanged(() => FilteredList);
			}
		}

		public List<InstallerItemViewModel> AllList { get; set; }

		public List<InstallerItemViewModel> FilteredList { get; private set; }

		//Pull to Refresh things
		private bool _isFetching;

		public bool IsFetching
		{
			get { return _isFetching; }
			set
			{
                _isFetching = value;
				RaisePropertyChanged(() => IsFetching);
			}
		}

		public bool IsCheckedIn
		{
			get { return Mvx.Resolve<IMobileDataService>().IsCheckedIn; }
			set
			{
				Mvx.Resolve<IMobileDataService>().IsCheckedIn = value;
				RaisePropertyChanged(() => IsCheckedIn);
			}
		}

		public NavigationFrom NavigationFrom { get; set; }
				
		private bool _upDateButton;

		public bool UpDateButton
		{
			get { return _upDateButton; }
			set
			{
				_upDateButton = value;
				RaisePropertyChanged(() => UpDateButton);

			}
		}



		private bool _navigateToMyWorkOrders;

		public bool NavigateToMyWorkOrders
		{
			get { return _navigateToMyWorkOrders; }

			set
			{
				_navigateToMyWorkOrders = value;
				RaisePropertyChanged(() => NavigateToMyWorkOrders);

			}
		}

		public string FilterBoxLocalization
		{
			get { return LanguageResolver.FilterBox; }
		}

		#endregion

		#region Public Methods
		
		public async Task GetInstallers()
		{
			if (!IsFetching)
				IsLoading = true;		

			var installers = await Mvx.Resolve<IMobileDataService>().GetInstallers();
		    ShowList(installers);
		}

        private void ShowList(List<ModelInstaller> installers)
        {
            AllList = Mapper.Map<List<ModelInstaller>, List<InstallerItemViewModel>>(installers);
            SearchString = string.Empty;
            RaisePropertyChanged(() => SearchString);
            RaisePropertyChanged(() => Count);           
            IsLoading = false;
        }

        public void ResetView()
		{
			RaisePropertyChanged(() => Count);			
		}	

		public void Clear()
		{

			foreach (var item in FilteredList)
			{
				item.Selected = false;
			}

			RaisePropertyChanged(() => FilteredList);
		}

		#endregion

		#region Commands

		private MvxAsyncCommand<InstallerItemViewModel> _installerSelectedCommand;

		public IMvxAsyncCommand InstallerSelectedCommand
        {
			get
			{
                _installerSelectedCommand = _installerSelectedCommand ?? new MvxAsyncCommand<InstallerItemViewModel>(DoSelectItem);
				return _installerSelectedCommand;
			}
		}
		
		#endregion

		#region Private Methods

	    private async Task DoSelectItem(InstallerItemViewModel customerItem)
	    {
	        await Task.Run(() =>
	        {
	            ShowViewModel<InstallerDetailsViewModel>(customerItem);
	        });
	    }	 
		#endregion

		#region Protected Methods

		public override async Task RefreshData()
		{
            IsFetching = true;
			Mvx.Resolve<IMobileDataService>().ForceFetch = true;
			await GetInstallers();
            IsFetching = false;
		}

		#endregion

	}
}

