using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mancoba.Sompisi.Core.Services.Contracts;
using Mancoba.Sompisi.Data.Models;
using Mancoba.Sompisi.Utils.Enums;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.ViewModels
{
	public class ProviderListViewModel : MessengerBaseViewModel
	{
		#region Constructor

		public ProviderListViewModel(IMvxMessenger messenger)
			: base(messenger)
		{
			
		}

	    public void Init(List<ModelProvider> providers)
	    {
            ShowList(providers);
        }

        #endregion

        #region Properties

        public string CancelLabel => LanguageResolver.Cancel.ToUpperInvariant();

        public string Count => string.Format(LanguageResolver.ProviderCountLabel, AllList.Count.ToString());

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

        public List<ProviderItemViewModel> AllList { get; set; }

        public List<ProviderItemViewModel> FilteredList { get; private set; }

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

        public async Task GetProviders()
		{
			if (!IsFetching)
				IsLoading = true;		
			
			var providers = await Mvx.Resolve<IMobileDataService>().GetProviders();
		    ShowList(providers);
		}

	    private void ShowList(List<ModelProvider> providers)
	    {
            AllList = Mapper.Map<List<ModelProvider>, List<ProviderItemViewModel>>(providers);
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

        private MvxAsyncCommand<ProviderItemViewModel> _installerSelectedCommand;

        public IMvxAsyncCommand ProviderSelectedCommand
        {
            get
            {
                _installerSelectedCommand = _installerSelectedCommand ?? new MvxAsyncCommand<ProviderItemViewModel>(DoSelectItem);
                return _installerSelectedCommand;
            }
        }

        #endregion

        #region Private Methods

        private async Task DoSelectItem(ProviderItemViewModel item)
        {
            await Task.Run(() =>
            {
                ShowViewModel<ProviderDetailsViewModel>(item);
            });
        }
        #endregion

        #region Protected Methods

        public override async Task RefreshData()
		{
            IsFetching = true;
			Mvx.Resolve<IMobileDataService>().ForceFetch = true;
			await GetProviders();
            IsFetching = false;
		}

		#endregion

	}
}

