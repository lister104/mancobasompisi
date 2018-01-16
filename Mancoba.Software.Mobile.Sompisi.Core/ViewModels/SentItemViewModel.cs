using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mancoba.Sompisi.Core.ViewModels
{
    public class SentItemViewModel : MvxViewModel
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

        public SentItemViewModel()
        {
            Initialise();
        }

        private void Initialise()
        {
            _firstName = "";
            _lastName = "";
            _phoneNumber = "";
            _mobileNumber = "";
            _emailAddress = "";

            _streetName = "";
            _suburbName = "";
            _townName = "";
            _provinceName = "";
            _countryName = "";
            _postalCode = "";
        }

        #region Data Properties		

        public string Id
        {
            get;
            set;
        }

        public string FirstName
        {
            get { return _firstName; }

            set
            {
                _firstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        public string LastName
        {
            get { return _lastName; }

            set
            {
                _lastName = value;
                RaisePropertyChanged(() => LastName);
            }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                RaisePropertyChanged(() => PhoneNumber);
            }
        }

        public string MobileNumber
        {
            get { return _mobileNumber; }
            set
            {
                _mobileNumber = value;
                RaisePropertyChanged(() => MobileNumber);
            }
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                _emailAddress = value;
                RaisePropertyChanged(() => EmailAddress);
            }
        }

        public string StreetName
        {
            get { return _streetName; }
            set
            {
                _streetName = value;
                RaisePropertyChanged(() => StreetName);
            }
        }

        public string SuburbName
        {
            get { return _suburbName; }
            set
            {
                _suburbName = value;
                RaisePropertyChanged(() => SuburbName);
            }
        }

        public string TownName
        {
            get { return _townName; }
            set
            {
                _townName = value;
                RaisePropertyChanged(() => TownName);
            }
        }

        public string ProvinceName
        {
            get { return _provinceName; }
            set
            {
                _provinceName = value;
                RaisePropertyChanged(() => ProvinceName);
            }
        }

        public string CountryName
        {
            get { return _countryName; }
            set
            {
                _countryName = value;
                RaisePropertyChanged(() => CountryName);
            }
        }

        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                _postalCode = value;
                RaisePropertyChanged(() => PostalCode);
            }
        }

        public string FullName => $"{FirstName} {LastName}";

        #endregion

        #region Label Properties

        public string FirstNameLabel => LanguageResolver.FirstNames;

        public string LastNameLabel => LanguageResolver.LastName;

        public string PhoneNumberLabel => LanguageResolver.PhoneNumber;

        public string MobileNumberLabel => LanguageResolver.MobileNumber;

        public string EmailAddressLabel => LanguageResolver.EmailAddress;

        public string StreetLabel => LanguageResolver.Street;

        public string SuburbLabel => LanguageResolver.Suburb;

        public string TownLabel => LanguageResolver.Town;

        public string ProvinceLabel => LanguageResolver.Province;

        public string CountryLabel => LanguageResolver.Country;

        public string PostalCodeLabel => LanguageResolver.PostalCode;

        #endregion

        #region Extra Properties	      

        public bool IsCheckedIn => true;

        private bool _selected;

        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;

                Drawable = _selected ? "mix_btn_check_on_holo_light" : "mix_btn_check_off_holo_light";

                RaisePropertyChanged(() => Selected);
            }
        }

        private string _drawable = "mix_btn_check_off_holo_light";

        public string Drawable
        {
            get { return _drawable; }
            set
            {
                _drawable = value;
                RaisePropertyChanged(() => Drawable);
            }
        }

        public bool IsHeading { get; set; }

        #endregion
    }
}
