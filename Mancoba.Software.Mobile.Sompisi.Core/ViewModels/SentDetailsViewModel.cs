using Mancoba.Sompisi.Core.Services.Contracts;
using Mancoba.Sompisi.Utils.Helpers.UserInteraction;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mancoba.Sompisi.Core.ViewModels
{
    public class SentDetailsViewModel : MessengerBaseViewModel
    {
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _mobileNumber;
        private string _emailAddress;
        
        private string _streetName;
        private string _suburbName;
        private string _townName;
        private string _provinceName;
        private string _countryName;
        private string _postalCode;

        private string _address;

        private bool _isProviderLoaded;
        private bool _isProductLoaded;

        #region Constructor

        public SentDetailsViewModel(IMvxMessenger messenger) : base(messenger)
        {

        }

        public void Init(SentItemViewModel item)
        {
            Id = item.Id;
        }

        #endregion

        #region Data Properties		

        public string Id
        {
            get;
            set;
        }

        public string FirstNameLabel => LanguageResolver.FirstNames;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        public string LastNameLabel => LanguageResolver.LastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged(() => LastName);
            }
        }

        public string PhoneNumberLabel => LanguageResolver.PhoneNumber;

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                RaisePropertyChanged(() => PhoneNumber);
            }
        }

        public string MobileNumberLabel => LanguageResolver.MobileNumber;

        public string MobileNumber
        {
            get { return _mobileNumber; }
            set
            {
                _mobileNumber = value;
                RaisePropertyChanged(() => MobileNumber);
            }
        }

        public string EmailAddressLabel => LanguageResolver.EmailAddress;

        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                _emailAddress = value;
                RaisePropertyChanged(() => EmailAddress);
            }
        }
        
        public string StreetLabel => LanguageResolver.Street;

        public string StreetName
        {
            get { return _streetName; }
            set
            {
                _streetName = value;
                RaisePropertyChanged(() => StreetName);
            }
        }

        public string SuburbLabel => LanguageResolver.Suburb;

        public string SuburbName
        {
            get { return _suburbName; }
            set
            {
                _suburbName = value;
                RaisePropertyChanged(() => SuburbName);
            }
        }

        public string TownLabel => LanguageResolver.Town;

        public string TownName
        {
            get { return _townName; }
            set
            {
                _townName = value;
                RaisePropertyChanged(() => TownName);
            }
        }

        public string ProvinceLabel => LanguageResolver.Province;

        public string ProvinceName
        {
            get { return _provinceName; }
            set
            {
                _provinceName = value;
                RaisePropertyChanged(() => ProvinceName);
            }
        }

        public string CountryLabel => LanguageResolver.Country;

        public string CountryName
        {
            get { return _countryName; }
            set
            {
                _countryName = value;
                RaisePropertyChanged(() => CountryName);
            }
        }

        public string PostalCodeLabel => LanguageResolver.PostalCode;

        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                _postalCode = value;
                RaisePropertyChanged(() => PostalCode);
            }
        }

        #endregion

        #region Commands

        public string BackButtonLabel => LanguageResolver.Back.ToUpperInvariant();

        private IMvxAsyncCommand _backCommand;

        public IMvxAsyncCommand BackCommand
        {
            get
            {
                _backCommand = _backCommand ?? new MvxAsyncCommand(DoBackCommand);
                return _backCommand;
            }
        }

        private async Task DoBackCommand()
        {
            await Task.Run(() =>
            {
                Close(this);
            });
        }

        public string FavouriteButtonLabel => LanguageResolver.Favourite.ToUpperInvariant();

        private IMvxAsyncCommand _makeFavourite;

        public IMvxAsyncCommand MakeFavouriteCommand
        {
            get
            {
                _makeFavourite = _makeFavourite ?? new MvxAsyncCommand(DoMakeFavourite);
                return _makeFavourite;
            }
        }

        private async Task DoMakeFavourite()
        {
            IsLoading = true;
            await Task.Run(async () =>
            {
                await Mvx.Resolve<IMobileDataService>().FavouriteProvider(Id);
                IsLoading = false;
                Close(this);
            });
        }

        private IMvxAsyncCommand _phoneNumberCommand;

        public IMvxAsyncCommand PhoneNumberCommand
        {
            get
            {
                _phoneNumberCommand = _phoneNumberCommand ?? new MvxAsyncCommand(DoPhoneNumberCommand);
                return _phoneNumberCommand;
            }
        }

        private async Task DoPhoneNumberCommand()
        {
            if (string.IsNullOrEmpty(PhoneNumber))
                return;
            await Mvx.Resolve<IUserInteraction>().AlertAsync(PhoneNumber, LanguageResolver.PhoneNumber, LanguageResolver.Ok);
        }

        #endregion

        #region Public Methods

        public async Task GetSentApplicationDetails()
        {
            _isProviderLoaded = false;
            CheckLoadingStatus();

            var model = await Mvx.Resolve<IMobileDataService>().GetSentApplication(Id);
            Id = model.Id;
            
            FirstName = model.FirstName;
            LastName = model.LastName;            
            PhoneNumber = model.PhoneNumber;
            MobileNumber = model.MobileNumber;
            EmailAddress = model.EmailAddress;

            StreetName = model.Street;
            SuburbName = model.Suburb;
            TownName = model.Town;
            ProvinceName = model.Province;
            CountryName = model.Country;
            PostalCode = model.PostalCode;

            _isProviderLoaded = true;
            CheckLoadingStatus();
        }

        public async Task GetSentApplications()
        {
            _isProductLoaded = false;
            CheckLoadingStatus();

            try
            {
                var applications = await Mvx.Resolve<IMobileDataService>().GetSentApplications();
                if (applications == null)
                    return;

                SentApplicationCollection.Clear();

                var head = new SentItemViewModel()
                {
                    IsHeading = true,
                    FirstName = "Brown",
                    LastName = "Njemza"
                };

                SentApplicationCollection.Add(head);

                foreach (var application in applications)
                {
                    var item = new SentItemViewModel()
                    {
                        IsHeading = false,
                        Id = application.Id,
                        FirstName = application.FirstName,
                        LastName = application.LastName
                    };

                    SentApplicationCollection.Add(item);
                }
                RaisePropertyChanged(() => SentApplicationCollection);
                HasFetched = true;
            }
            finally
            {
                _isProductLoaded = true;
                CheckLoadingStatus();
            }
        }

        private void CheckLoadingStatus()
        {
            IsLoading = true;
            if (_isProviderLoaded && _isProductLoaded)
                IsLoading = false;
        }

        public void ReloadSentApplicationCollection()
        {
            RaisePropertyChanged(() => SentApplicationCollection);
        }

        #endregion

        #region Applications

        private List<SentItemViewModel> _sentApplicationCollection = new List<SentItemViewModel>();

        public List<SentItemViewModel> SentApplicationCollection
        {
            get { return _sentApplicationCollection; }
            set
            {
                _sentApplicationCollection = value;
                RaisePropertyChanged(() => SentApplicationCollection);
            }
        }

        #endregion
        
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

        #region Labels

        public string SentApplicationInfoHeading = "Sent Applications Info".ToUpperInvariant();

        public string AddressInforHeading = "Address Info".ToUpperInvariant();

        public string ApplicationInfoHeading => "Applications sent".ToUpperInvariant();

        #endregion
    }
}

