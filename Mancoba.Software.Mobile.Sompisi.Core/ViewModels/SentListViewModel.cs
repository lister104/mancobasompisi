using AutoMapper;
using Mancoba.Sompisi.Core.Services.Contracts;
using Mancoba.Sompisi.Data.Models;
using Mancoba.Sompisi.Utils.Enums;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mancoba.Sompisi.Core.ViewModels
{
    public class SentListViewModel : MessengerBaseViewModel
    {
        #region Constructors

        public SentListViewModel(IMvxMessenger messenger)
            : base(messenger)
        {

        }

        public void Init(List<ModelApplication> applicationsSent)
        {
            ShowList(applicationsSent);
        }

        #endregion

        private string _searchString;

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;

                if (string.IsNullOrEmpty(value))
                {

                    FilteredList = ApplicationList;
                }
                else
                {
                    FilteredList = ApplicationList.Where(o => o.FirstName.ToLowerInvariant().Contains(value.ToLowerInvariant()) ||
                                                        o.LastName.ToLowerInvariant().Contains(value.ToLowerInvariant()) ||
                                                        o.MobileNumber.ToLowerInvariant().Contains(value.ToLowerInvariant())).ToList();
                }

                RaisePropertyChanged(() => SearchString);
                RaisePropertyChanged(() => FilteredList);
            }
        }

        public string FilterBoxLocalization
        {
            get { return LanguageResolver.FilterBox; }
        }

        public List<SentItemViewModel> ApplicationList { get; set; }

        public List<SentItemViewModel> FilteredList { get; private set; }

        public string Count => string.Format(LanguageResolver.ApplicationCountLabel, ApplicationList.Count.ToString());

        private void ShowList(List<ModelApplication> applicationsSent)
        {
            ApplicationList = Mapper.Map<List<ModelApplication>, List<SentItemViewModel>>(applicationsSent);
            SearchString = string.Empty;
            RaisePropertyChanged(() => SearchString);
            RaisePropertyChanged(() => Count);
            IsLoading = false;
        }

        public NavigationFrom NavigationFrom { get; set; }

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

        public async Task GetSentApplications()
        {
            if (!IsFetching)
                IsLoading = true;

            var sentItems = await Mvx.Resolve<IMobileDataService>().GetSentApplications();
            ShowList(sentItems);
        }

        #region Private Methods

        private async Task DoSelectItem(SentItemViewModel item)
        {
            await Task.Run(() =>
            {
                ShowViewModel<SentItemViewModel>(item);
            });
        }

        #endregion

        #region Protected Methods

        public override async Task RefreshData()
        {
            IsFetching = true;
            Mvx.Resolve<IMobileDataService>().ForceFetch = true;
            await GetSentApplications();
            IsFetching = false;
        }

        #endregion
    }
}
