using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bluescore.DStv.Core.Services.Contracts;
using Bluescore.DStv.Core.Validators;
using Bluescore.DStv.Core.ViewModels.Base;
using Bluescore.DStv.Data.Models;
using Bluescore.DStv.Utils.Helpers;
using Bluescore.DStv.Utils.Helpers.UserInteraction;
using Bluescore.DStv.Utils.Language;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Bluescore.DStv.Core.ViewModels
{
    public class AddressAddViewModel : BaseValidationViewModel
    {
        #region Private Variables		

        private readonly IMobileDataService _dataService;
        private readonly AddressAddValidator _validator;
       
        private ModelTown _townModel;
        private ModelSuburb _suburbModel;
        private ModelStreet _streetModel;

        private bool _isWidgetHidden;
        private bool _isWidgetVisible;
       
        private string _province;
        private string _town;
        private string _suburb;      
        private string _addLocation;

        private string _locationHeading;

        #endregion

        public AddressAddViewModel(IMvxMessenger messenger, IMobileDataService dataService) : base(messenger)
        {
            Initialise();

            _dataService = dataService;
            _validator = new AddressAddValidator();
           
            UpdateProvincesItemsSource();
            UpdateTownsItemsSource();
            UpdateSuburbsItemsSource();           
        }

        private void Initialise()
        {           
            _province = "";
            _town = "";
            _suburb = "";          
            _addLocation = "";           
        }       

        #region Data Properties

        public string Id { get; set; }
       
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

        public string AddLocation
        {
            get { return _addLocation; }

            set
            {
                _addLocation = value;              
                RaisePropertyChanged(() => AddLocation);
            }

        }

        #endregion

        #region Heading Properties

        public string CustomerInforHeading => LanguageResolver.CustomerInfor.ToUpperInvariant();
        public string AddressInforHeading => LanguageResolver.AddressInfor.ToUpperInvariant();
       
        public string ProvincePlaceholder => LanguageResolver.Province;
        public string TownPlaceholder => LanguageResolver.Town;
        public string SuburbPlaceholder => LanguageResolver.Suburb;

        public string AddLocationPlaceholder
        {
            get { return _locationHeading; }

            set
            {
                _locationHeading = value;
                RaisePropertyChanged(() => AddLocationPlaceholder);
            }

        }

        #endregion

        #region Commands

        public string CancelButtonLabel => LanguageResolver.Cancel;
        public string SaveButtonLabel => LanguageResolver.Save;

        private IMvxAsyncCommand _cancelCommand;

        public IMvxAsyncCommand CancelCommand
        {
            get
            {
                _cancelCommand = _cancelCommand ?? new MvxAsyncCommand(CancelCommandHandler);
                return _cancelCommand;
            }
        }

        private IMvxAsyncCommand _addCommand;

        public IMvxAsyncCommand AddCommand
        {
            get
            {
                _addCommand = _addCommand ?? new MvxAsyncCommand(AddCommandHandler);
                return _addCommand;
            }
        }

        private const string AddTown = "Add a new Town";
        private const string AddSuburb = "Add a new Suburb";
        private const string AddStreet = "Add a new Street";
        public async Task UpdateHeading(int level = 0)
        {
           Task.Run(() =>
           {
               string msg = AddTown;
                if (level > 0)
                {
                   if(level == 1)
                       msg = AddTown;
                   if (level == 2)
                       msg = AddSuburb;
                   if (level == 3)
                       msg = AddStreet;
               }
                else
                {
                    if (TownSelectedItem != null && !string.IsNullOrWhiteSpace(TownSelectedItem.Id))
                        msg = AddSuburb;

                    if (SuburbSelectedItem != null && !string.IsNullOrWhiteSpace(SuburbSelectedItem.Id))
                        msg = AddStreet;
                }

                AddLocationPlaceholder = msg;
            });
        }

        private async Task CancelCommandHandler()
        {
            Task.Run(() =>
            {
                Close(this);
                ShowViewModel<ProviderSearchViewModel>();
            });
        }

        public async Task AddCommandHandler()
        {
            ValidationErrors?.Clear();

            ValidateModel(_validator, this);

            if (IsModelValid)
            {
                IsLoading = true;

                bool success = true; //await _dataService.SaveCustomer(_customer);

                if (success)
                {
                    ShowViewModel<ProviderSearchViewModel>();
                }
                else
                {
                    await
                        Mvx.Resolve<IUserInteraction>()
                            .AlertAsync(LanguageResolver.SearchErrorTitle, "No Customers Found");
                }

                IsLoading = false;
            }
            else
            {
                string errorMsg = string.Empty;

                if (ValidationErrors != null && ValidationErrors.Count > 0)
                {
                    errorMsg = ValidationErrors.First().ErrorMessage;
                }
                else
                {
                    errorMsg = "An error has occurred.";
                }

                Mvx.Resolve<IUserInteraction>().ToastErrorAlert(errorMsg);
            }
        }

        #endregion

        #region Public Methods

        public bool IsWidgetHidden
        {
            get { return _isWidgetHidden; }
            set
            {
                _isWidgetHidden = value;
                RaisePropertyChanged(() => IsWidgetHidden);
            }
        }

        public bool IsWidgetVisible
        {
            get { return _isWidgetVisible; }
            set
            {
                _isWidgetVisible = value;
                RaisePropertyChanged(() => IsWidgetVisible);
            }
        }

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

        #region DropDownLists     

        #region Provinces
        private DropDownListItem _provinceSelectedItem;
        private List<DropDownListItem> _provinceTitleItems = new List<DropDownListItem>();
        private async Task UpdateProvincesItemsSource()
        {
            ProvinceItemsSource = await _dataService.GetDropDownProvinces();
        }

        public List<DropDownListItem> ProvinceItemsSource
        {
            get { return _provinceTitleItems; }
            set { _provinceTitleItems = value; RaisePropertyChanged(() => ProvinceItemsSource); }
        }

        public DropDownListItem ProvinceSelectedItem
        {
            get
            {
                if (_provinceSelectedItem == null && _provinceTitleItems.Count > 0)
                    _provinceSelectedItem = _provinceTitleItems[0];

                return _provinceSelectedItem;
            }
            set
            {
                _provinceSelectedItem = value;              
                RaisePropertyChanged(() => ProvinceSelectedItem);               
            }
        }

        #endregion

        #region Towns
        private DropDownListItem _townSelectedItem;
        private List<DropDownListItem> _townItems = new List<DropDownListItem>();
        public async Task UpdateTownsItemsSource(string id = "")
        {
            if (string.IsNullOrWhiteSpace(id))
                id = ProvinceSelectedItem.Id;

            TownItemsSource = await _dataService.GetDropDownTownsByProvice(id);           
        }

        public List<DropDownListItem> TownItemsSource
        {
            get { return _townItems; }
            set { _townItems = value; RaisePropertyChanged(() => TownItemsSource); }
        }

        public DropDownListItem TownSelectedItem
        {
            get
            {
                if (_townSelectedItem == null && _townItems.Count > 0)
                    _townSelectedItem = _townItems[0];

                return _townSelectedItem;
            }
            set
            {
                _townSelectedItem = value;              
                RaisePropertyChanged(() => TownSelectedItem);               
            }
        }

        #endregion

        #region Suburbs
        private DropDownListItem _suburbSelectedItem;
        private List<DropDownListItem> _suburbItems = new List<DropDownListItem>();
        public async Task UpdateSuburbsItemsSource(string id = "")
        {
            if (string.IsNullOrWhiteSpace(id))
                id = TownSelectedItem.Id;

            SuburbItemsSource = await _dataService.GetDropDownSuburbsByTown(id);
        }

        public List<DropDownListItem> SuburbItemsSource
        {
            get { return _suburbItems; }
            set { _suburbItems = value; RaisePropertyChanged(() => SuburbItemsSource); }
        }

        public DropDownListItem SuburbSelectedItem
        {
            get
            {
                if (_suburbSelectedItem == null && _suburbItems.Count > 0)
                    _suburbSelectedItem = _suburbItems[0];

                return _suburbSelectedItem;
            }
            set
            {
                _suburbSelectedItem = value;              
                RaisePropertyChanged(() => SuburbSelectedItem);               
            }
        }

        #endregion
         

        #endregion
    }
}

