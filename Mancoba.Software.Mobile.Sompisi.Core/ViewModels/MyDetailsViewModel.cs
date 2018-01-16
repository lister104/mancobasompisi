using System.Threading.Tasks;
using Mancoba.Sompisi.Core.Services.Contracts;
using Mancoba.Sompisi.Core.ViewModels.Base;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.ViewModels
{
    public class MyDetailsViewModel : BaseValidationViewModel
    {
        #region Private Variables		

        private readonly IMobileDataService _dataService;     
      
		private string _username; 
		private string _lastName;
        private string _firstName;
		private string _phoneNumber;
		private string _mobileNumber;		
		private string _emailAddress;

		private string _idNumber;      

        private string _province;
        private string _town;
        private string _suburb;
        private string _street;
        private string _houseName;
    
       
        #endregion

        public MyDetailsViewModel(IMvxMessenger messenger, IMobileDataService dataService) : base(messenger)
        {
            Initialise();
            _dataService = dataService;           
        }

        private void Initialise()
        {
            _lastName = "";
            _firstName = "";
            _idNumber = "";          
            _province = "";
            _town = "";
            _suburb = "";
            _street = "";
			_houseName = "";
           
        }

        public async Task LoadSystemUser()
        {			
			var model = await Mvx.Resolve<IMobileDataService>().GetSystemUser();
			
			Id = model.Id;
			UserName = model.Username;
			FirstName = model.FirstName;
			LastName = model.LastName;
			
			EmailAddress = model.EmailAddress;
			PhoneNumber = model.PhoneNumber;
			MobileNumber = model.MobileNumber;

			Province = model.Province;
			Town = model.Town;
			Suburb = model.Suburb;
			Street = model.Street;
			HouseName = model.House;
														
		}

        #region Data Properties
  
        public string Id { get; set; }
	
		public string UserName
		{
			get { return _username; }

			set
			{
				_username = value;
				RaisePropertyChanged(() => UserName);
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

        public string FirstName
        {
            get { return _firstName; }

            set
            {
                _firstName = value;              
                RaisePropertyChanged(() => FirstName);
            }

        }

        public string IdNumber
        {
            get { return _idNumber; }

            set
            {
                _idNumber = value;               
                RaisePropertyChanged(() => IdNumber);
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

        public string Province
        {
            get { return _province; }

            set
            {
                _province = value;               
                RaisePropertyChanged(() => Province);
            }

        }


        public string Town
        {
            get { return _town; }

            set
            {
                _town = value;              
                RaisePropertyChanged(() => Town);
            }

        }

        public string Suburb
        {
            get { return _suburb; }

            set
            {
                _suburb = value;             
                RaisePropertyChanged(() => Suburb);
            }

        }

        public string Street
        {
            get { return _street; }

            set
            {
                _street = value;              
                RaisePropertyChanged(() => Street);
            }

        }

        public string HouseName
        {
            get { return _houseName; }

            set
            {
				_houseName = value;              
                RaisePropertyChanged(() => HouseName);
            }
        }

        #endregion

        #region Heading Properties

        public string MyProfileInforHeading => "My Details".ToUpperInvariant();
        public string AddressInforHeading => LanguageResolver.AddressInfor.ToUpperInvariant();

		public string UserNamePlaceholder => LanguageResolver.UserName;
		public string LastNamePlaceholder => LanguageResolver.LastName;
        public string FirstNamesPlaceholder => LanguageResolver.FirstNames;
        public string PhoneNumberPlaceholder => LanguageResolver.PhoneNumber;
		public string MobileNumberPlaceholder => LanguageResolver.MobileNumber;
		public string EmailAddressPlaceholder => LanguageResolver.EmailAddress;
		public string IdNumberPlaceholder => LanguageResolver.IdNumber;
        public string AccountNumberPlaceholder => LanguageResolver.AccountNumber;
        public string ProvincePlaceholder => LanguageResolver.Province;
        public string TownPlaceholder => LanguageResolver.Town;
        public string SuburbPlaceholder => LanguageResolver.Suburb;
        public string StreetPlaceholder => LanguageResolver.Street;
        public string HousePlaceholder => LanguageResolver.Address;

        #endregion
     

        #region Public Methods

	    public bool IsWidgetHidden = false;

	    public bool IsWidgetVisible = true;

        public void Init()
        {
            SetWidgetsVisiblity(true);
        }

        private void SetWidgetsVisiblity(bool visible)
        {
            IsWidgetVisible = visible;
            IsWidgetHidden = !visible;
        }

        #endregion
       
    }
}

