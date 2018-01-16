using AutoMapper;
using Mancoba.Sompisi.Core.Services.Contracts;
using Mancoba.Sompisi.Data.Models;
using Mancoba.Sompisi.Utils.Enums;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mancoba.Sompisi.Core.ViewModels
{
    public class SentListViewModel : MessengerBaseViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SentListViewModel" /> class.
        /// </summary>
        /// <param name="messenger">The messenger.</param>
        public SentListViewModel(IMvxMessenger messenger)
                    : base(messenger)
        {

        }

        /// <summary>
        /// Initializes the specified applications sent.
        /// </summary>
        /// <param name="applicationsSent">The applications sent.</param>
        public void Init(List<ModelApplication> applicationsSent)
        {
            ShowList(applicationsSent);
        }

        #endregion

        private string _searchString;

        /// <summary>
        /// Gets or sets the search string.
        /// </summary>
        /// <value>
        /// The search string.
        /// </value>
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

        /// <summary>
        /// Shows the list.
        /// </summary>
        /// <param name="applicationsSent">The applications sent.</param>
        private void ShowList(List<ModelApplication> applicationsSent)
        {
            ApplicationList = Mapper.Map<List<ModelApplication>, List<SentItemViewModel>>(applicationsSent);
            SearchString = string.Empty;
            RaisePropertyChanged(() => SearchString);
            RaisePropertyChanged(() => Count);
            IsLoading = false;
        }

        public NavigationFrom NavigationFrom { get; set; }

        /// <summary>
        /// Resets the view.
        /// </summary>
        public void ResetView()
        {
            RaisePropertyChanged(() => Count);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            foreach (var item in FilteredList)
            {
                item.Selected = false;
            }
            RaisePropertyChanged(() => FilteredList);
        }

        private bool _isFetching;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is fetching.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is fetching; otherwise, <c>false</c>.
        /// </value>
        public bool IsFetching
        {
            get { return _isFetching; }
            set
            {
                _isFetching = value;
                RaisePropertyChanged(() => IsFetching);
            }
        }

        private MvxAsyncCommand<SentItemViewModel> _applicationSelectedCommand;

        /// <summary>
        /// Gets the application selected command.
        /// </summary>
        /// <value>
        /// The application selected command.
        /// </value>
        public IMvxAsyncCommand ApplicationSelectedCommand
        {
            get
            {
                _applicationSelectedCommand = _applicationSelectedCommand ?? new MvxAsyncCommand<SentItemViewModel>(DoSelectItem);
                return _applicationSelectedCommand;
            }
        }

        /// <summary>
        /// Gets the sent applications.
        /// </summary>
        /// <returns></returns>
        public async Task GetSentApplications()
        {
            if (!IsFetching)
                IsLoading = true;

            var sentItems = await Mvx.Resolve<IMobileDataService>().GetSentApplications();
            ShowList(sentItems);
        }

        #region Private Methods

        /// <summary>
        /// Does the select item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private async Task DoSelectItem(SentItemViewModel item)
        {
            await Task.Run(() =>
            {
                ShowViewModel<SentItemViewModel>(item);
            });
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Refreshes the data.
        /// </summary>
        /// <returns></returns>
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
