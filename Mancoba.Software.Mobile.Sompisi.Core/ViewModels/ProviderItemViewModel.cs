using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Core.ViewModels;

namespace Mancoba.Sompisi.Core.ViewModels
{
    public class ProviderItemViewModel : MvxViewModel
    {
        private string _name;

        private string _contactPerson;
        private string _phoneNumber;
        private string _mobileNumber;
        private string _webAddress;
        private string _emailAddress;

        private string _address;

        public ProviderItemViewModel()
        {
            Initialise();
        }

        private void Initialise()
        {
            _name = "";

            _contactPerson = "";
            _phoneNumber = "";
            _mobileNumber = "";
            _webAddress = "";
            _emailAddress = "";

            _address = "";
        }

        #region Data Properties		
        public string Id
        {
            get;
            set;
        }
       
        public string Name
        {
            get { return _name; }

            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public string ContactPerson
        {
            get { return _contactPerson; }

            set
            {
                _contactPerson = value;
                RaisePropertyChanged(() => ContactPerson);
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

        public string WebAddress
        {
            get { return _webAddress; }

            set
            {
                _webAddress = value;
                RaisePropertyChanged(() => WebAddress);
            }
        }

        public string Address
        {
            get { return _address; }

            set
            {
                _address = value;
                RaisePropertyChanged(() => Address);
            }
        }

        public string FullName => $"{Name}";

        #endregion

        #region Label Properties
        public string NameLabel => LanguageResolver.Name;

        public string ContactLabel => LanguageResolver.Contact;

        public string PhoneNumberLabel => LanguageResolver.PhoneNumber;

        public string EmailAddressLabel => LanguageResolver.EmailAddress;

        public string AddressLabel => LanguageResolver.Address;

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


        #endregion
    }
}

