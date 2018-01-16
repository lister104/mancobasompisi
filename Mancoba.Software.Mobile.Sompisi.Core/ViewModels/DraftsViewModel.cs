using Mancoba.Sompisi.Core.Services.Contracts;
using Mancoba.Sompisi.Data.Models;
using Mancoba.Sompisi.Utils.Helpers.UserInteraction;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using System;
using System.Threading.Tasks;

namespace Mancoba.Sompisi.Core.ViewModels
{
    public class DraftsViewModel : MessengerBaseViewModel
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

        private bool _isDraftViewLoaded;
        public string ApplicationInforHeading = "Application Info".ToUpperInvariant();
        public string AddressInforHeading = "Address Info".ToUpperInvariant();

        #region Constructors

        public DraftsViewModel(IMvxMessenger messenger)
            : base(messenger)
        {

        }

        public void Init()
        {

        }

        public void Init(DraftItemViewModel item)
        {
            FirstName = item.FirstName;
            LastName = item.LastName;
            PhoneNumber = item.PhoneNumber;
            MobileNumber = item.MobileNumber;
            EmailAddress = item.EmailAddress;

            StreetName = item.StreetName;
            SuburbName = item.SuburbName;
            TownName = item.TownName;
            ProvinceName = item.ProvinceName;
            CountryName = item.CountryName;
            PostalCode = item.PostalCode;
        }

        #endregion

        #region Data Properties

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

        private bool _isHeading;
        public bool IsHeading
        {
            get { return _isHeading; }
            set
            {
                _isHeading = value;
                RaisePropertyChanged(() => IsHeading);
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

        public string SaveButtonLabel => LanguageResolver.Save.ToUpperInvariant();

        private IMvxAsyncCommand _saveApplication;

        public IMvxAsyncCommand SaveApplicationCommand
        {
            get
            {
                _saveApplication = _saveApplication ?? new MvxAsyncCommand(DoSaveApplication);
                return _saveApplication;
            }
        }

        private async Task DoSaveApplication()
        {
            IsLoading = true;
            await Task.Run(async () =>
            {
                ModelApplication model = PopulateApplication();
                await Mvx.Resolve<IMobileDataService>().SaveApplication(model);
                IsLoading = false;
                Close(this);
            });
        }

        private ModelApplication PopulateApplication()
        {
            return new ModelApplication
            {
                FirstName = FirstName,
                LastName = LastName,
                PhoneNumber = PhoneNumber,
                MobileNumber = MobileNumber,
                EmailAddress = EmailAddress,
                Street = StreetName,
                Suburb = SuburbName,
                Town = TownName,
                Province = ProvinceName,
                Country = CountryName,
                PostalCode = PostalCode
            };
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

        public async Task GetDraftDetails()
        {
            _isDraftViewLoaded = false;
            IsLoading = true;

            //var model = await Mvx.Resolve<IMobileDataService>().GetDraft(Id);
            //Id = model.Id;
            //For now
            await Task.FromResult(true);
            
            FirstName = "Brown";
            LastName = "Njemza";

            PhoneNumber = "+27 21 113 8728";
            MobileNumber = "+27 76 113 8728";
            EmailAddress = "njemza@gmail.com";            

            StreetName = "104 Njembi Street";
            SuburbName = "Hagley";
            TownName = "Kuilsriver";
            ProvinceName = "Western Cape";
            CountryName = "South Africa";
            PostalCode = "7580";

            _isDraftViewLoaded = true;
            CheckLoadingStatus();
        }

        private void CheckLoadingStatus()
        {
            IsLoading = true;
            if (_isDraftViewLoaded)
                IsLoading = false;
        }

        #endregion
    }
}
