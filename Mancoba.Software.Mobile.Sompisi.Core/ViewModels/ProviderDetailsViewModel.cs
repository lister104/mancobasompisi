using System.Collections.Generic;
using System.Threading.Tasks;
using Mancoba.Sompisi.Core.Services.Contracts;
using Mancoba.Sompisi.Utils.Helpers;
using Mancoba.Sompisi.Utils.Helpers.UserInteraction;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.ViewModels
{
	public class ProviderDetailsViewModel : MessengerBaseViewModel
	{
		private string _name;

		private string _contactPerson;
		private string _phoneNumber;
		private string _mobileNumber;
		private string _webAddress;
		private string _emailAddress;

		private string _countryName;
		private string _provinceName;
		private string _townName;
		private string _suburbName;
		private string _streetName;
		private string _homeName;

		private string _address;

		private bool _isProviderLoaded;
        private bool _isProductLoaded;

        #region Constructor

        public ProviderDetailsViewModel(IMvxMessenger messenger)
			: base(messenger)
		{

		}

		public void Init(ProviderItemViewModel item)
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

        public string NameLabel => LanguageResolver.Name;
        public string Name
        {
            get { return _name; }

            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }

        }

        public string ContactPersonLabel => LanguageResolver.ContactPerson;
        public string ContactPerson
        {
            get { return _contactPerson; }

            set
            {
                _contactPerson = value;
                RaisePropertyChanged(() => ContactPerson);
            }

        }

        public string WebAddressLabel => LanguageResolver.WebSite;
        public string WebAddress
        {
            get { return _webAddress; }

            set
            {
                _webAddress = value;
                RaisePropertyChanged(() => WebAddress);
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

        public string HouseLabel => LanguageResolver.Address;
        public string HouseName
        {
            get { return _homeName; }

            set
            {
                _homeName = value;
                RaisePropertyChanged(() => HouseName);
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

		public async Task GetProviderDetails()
		{
			_isProviderLoaded = false;
		    CheckLoadingStatus();

            var model = await Mvx.Resolve<IMobileDataService>().GetProvider(Id);
			Id = model.Id;
			ContactPerson = model.ContactPerson;
			Name = model.Name;

			EmailAddress = model.EmailAddress;
			PhoneNumber = model.PhoneNumber;
			MobileNumber = model.MobileNumber;
			WebAddress = model.WebAddress;

            HouseName = model.House;
            StreetName = model.Street;
            SuburbName = model.Suburb;
            TownName = model.Town;
            ProvinceName = model.Province;
            CountryName = model.Country;

            _isProviderLoaded = true;
			CheckLoadingStatus();
		}

        public async Task GetProductDetails()
        {
            _isProductLoaded = false;
            CheckLoadingStatus();

            try
            {				
				var assets = await Mvx.Resolve<IMobileDataService>().GetProducts();
                if (assets == null)
                    return;

				ProductCollection.Clear();

				var head = new ProductItemViewModel()
                {
                    IsHeading = true,
                    Name = "Product Name",
                    Price = "Price"
                };

                ProductCollection.Add(head);

                foreach (var asset in assets)
                {
                    var item = new ProductItemViewModel()
                    {
                        IsHeading = false,

                        Id = asset.Id,
                        Name = asset.Name,
                        Description = asset.Description,
                        Price = $"{asset.Price:C}",
                    };

                    ProductCollection.Add(item);
                }
                RaisePropertyChanged(() => ProductCollection);
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

		public void ReloadAssetCollection()
		{
			RaisePropertyChanged(() => ProductCollection);
		}

		#endregion

		#region Product
		

		private List<ProductItemViewModel> _productCollection = new List<ProductItemViewModel>();

		public List<ProductItemViewModel> ProductCollection
		{
			get { return _productCollection; }
			set
			{
				_productCollection = value;
				RaisePropertyChanged(() => ProductCollection);
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
        public string ProviderInforHeading = "Provider Info".ToUpperInvariant();
        public string AddressInforHeading = "Address Info".ToUpperInvariant();       
		public string ProductInforHeading => "Available Products".ToUpperInvariant();
        #endregion
    }
}

