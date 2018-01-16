using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bluescore.DStv.Core.Language;
using Bluescore.DStv.Core.Services.Contracts;
using Bluescore.DStv.Core.ViewModels;
using Bluescore.DStv.Data.Models;
using Bluescore.DStv.Utils.Enums;
using Bluescore.DStv.Utils.Helpers;
using Bluescore.DStv.Utils.Helpers.UserInteraction;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Plugin.Connectivity;

namespace Bluescore.DStv.Core.Services
{
    public class DataService : IDataService
    {
        private readonly IBluescoreTvWebApi _apiDataService;
        private List<CustomerItemViewModel> _customerItemList;

        public DataService(IBluescoreTvWebApi apiDataService)
        {
            _apiDataService = apiDataService;
        }

        public async Task<bool> CheckIn()
        {
            try
            {

                await _apiDataService.CheckIn();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CheckOut()
        {
            try
            {
                await _apiDataService.CheckOut();

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task AcceptWorkOrder(string orderId)
        {
            bool success = false;
            if (!Connected)
                return;

            try
            {

                success = await _apiDataService.AcceptWorkOrder(orderId);


            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
            finally
            {

                if (success)
                {
                    var myOrder = _customerItemList.FirstOrDefault((m) => m.Id.Equals(orderId));
                    if (myOrder != null)
                        myOrder.IsMyOrder = true;
                    Mvx.Resolve<IUserInteraction>().ToastAlert(LanguageResolver.OrderAccepted, ToastGravity.Bottom);
                }
                else
                    Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.OrderNotAccepted);
            }
        }

        public async Task DeAcceptWorkOrder(IEnumerable orderIds)
        {
            if (!Connected)
                return;

            bool allsucceeded = true;
            foreach (string orderId in orderIds)
            {

                bool success = false;
                try
                {

                    success = await _apiDataService.DeAcceptWorkOrder(orderId);


                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleError(ex);
                }
                finally
                {

                    if (success)
                    {

                        var myOrder = _customerItemList.FirstOrDefault((m) => m.Id.Equals(orderId));
                        if (myOrder != null)
                            myOrder.IsMyOrder = true;

                    }
                    else
                    {

                        allsucceeded = false;
                        Mvx.Resolve<IUserInteraction>()
                            .ToastErrorAlert(string.Format(LanguageResolver.OrderNotDeAccepted, orderId));

                    }
                }

            }

            if (allsucceeded)
                Mvx.Resolve<IUserInteraction>().ToastAlert(LanguageResolver.OrderDeAccepted, ToastGravity.Bottom);
        }

        public async Task<AssetInstallSummaryViewModel> FetchAssetInstallSummary()
        {
            if (!Connected)
                return null;

            AssetInstallSummaryViewModel vm = new AssetInstallSummaryViewModel(Mvx.Resolve<IMvxMessenger>());
            vm.AssetId = "345";
            vm.ConfigGroup = "Mix Staff Vehicles";
            vm.ConfigStatus = "Ready";
            vm.DeviceSerial = "34234534345345";
            vm.DriverManagement = "No driver socket";
            vm.MobileDeviceType = "MiX2000 GTR";
            vm.SpecialInstructions =
                "Please note that this is a test message and will go away once the real data is available. Thank you and calm down...";
            vm.StarterCut = "Yes";

            return vm;
        }

        public async Task<ModelAssetDetails> GetWorkOrderDetails(string workOrderId)
        {
            if (!Connected)
                return null;

            try
            {
                return await _apiDataService.GetAssetDetailsById(workOrderId);
                //return Mapper.Map<SalesForceWorkOrderDetailsModel, CustomerDetailsViewModel> (order);

            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
                return null;
            }
        }

        public async Task<InstallSummaryModel> GetInstallSummary(string orgId, string assetId)
        {
            if (!Connected)
                return null;

            try
            {
                return await _apiDataService.GetInstallSummary(orgId, assetId);

            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
                return null;
            }
        }

        public async Task<AssetModel> GetAssetSummary(string id)
        {
            if (!Connected)
                return null;
            try
            {

                return await _apiDataService.GetAssetSummary(id);
                //return Mapper.Map<SalesForceWorkOrderDetailsModel, CustomerDetailsViewModel> (order);

            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
                return null;
            }
        }


        #region Properties

        public bool IsCheckedIn { get; set; }

        public bool ForceFetch { get; set; }

        public bool Connected
        {

            get
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.NoConnectivityCannotUpdate);
                    return false;
                }
                return true;

            }
        }

        #endregion

        #region Customers	

        public bool HasFoundCustomers()
        {
            return (_customerItemList != null && _customerItemList.Count > 0);
        }

        public void ClearCustomerList()
        {
            Task.Run(async () =>
            {
                await Task.Run(() => { _customerItemList = null; });
            });
        }

        public async Task<ModelCustomerDetails> GetCustomerDetails(string id)
        {
            if (!Connected)
                return null;

            try
            {
                return await _apiDataService.GetCustomerDetailsById(id);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
                return null;
            }
        }

        public async Task<bool> FindCustomers(string searchText)
        {
            if (!Connected)
                return false;

            try
            {
                var data = await _apiDataService.FindCustomers(searchText);
                _customerItemList = Mapper.Map<IList<ModelCustomerDetails>, List<CustomerItemViewModel>>(data);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                ForceFetch = false;
            }

            return HasFoundCustomers();
        }

        public async Task<List<CustomerItemViewModel>> GetFoundCustomers()
        {
            return await Task.Run(() => _customerItemList);
        }

        public async Task<bool> SaveCustomer(ModelCustomerDetails customer)
        {
            var success = false;

            try
            {
                if (!Connected)
                    return false;

                success = await _apiDataService.SaveCustomer(customer);

            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
                Mvx.Resolve<IUserInteraction>().ToastErrorAlert(ex.ToString());
            }

            return success;
        }

        #endregion

        #region Addresses

        private static List<ModelProvince> _provinceList;
        private static List<ModelTown> _townList;
        private static List<ModelSuburb> _suburbList;
        private static List<ModelStreet> _streetList;

        private static List<DropDownListItem> _provinceDropDownList;
        private static List<DropDownListItem> _townDropDownList;
        private static List<DropDownListItem> _suburbDropDownList;
        private static List<DropDownListItem> _streetDropDownList;

        public async Task<List<ModelProvince>> GetProvinces()
        {
            if (_provinceList == null)
                _provinceList = new List<ModelProvince>();

            if (_provinceDropDownList == null)
                _provinceDropDownList = new List<DropDownListItem>();

            try
            {
                if (_provinceList.Count == 0)                
                    _provinceList = await _apiDataService.GetProvinces();                   
                
                if (_provinceDropDownList.Count == 0)
                {                   
                    _provinceDropDownList.Add(new DropDownListItem(string.Empty, "Please select a Province", string.Empty));

                    foreach (var m in _provinceList)
                    {
                        _provinceDropDownList.Add(new DropDownListItem(m.Id, m.Name, m.Country));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return _provinceList.OrderBy(c => c.Name).ToList();
        }

        public async Task<List<DropDownListItem>> GetDropDownProvinces()
        {            
            if (_provinceDropDownList == null)
                _provinceDropDownList = new List<DropDownListItem>();

            if (_provinceDropDownList.Count == 0)            
                await GetProvinces();                
            
            return _provinceDropDownList;
        }

        public async Task<List<ModelTown>> GetTowns()
        {
            await GetProvinces();

            if (_townList == null)
                _townList = new List<ModelTown>();

            if (_townDropDownList == null)
                _townDropDownList = new List<DropDownListItem>();
            try
            {

                if (_townList.Count == 0)
                    _townList = await _apiDataService.GetTowns();

                if (_townDropDownList.Count == 0)
                {
                    _townList = _townList.OrderBy(c => c.Name).ToList();
                    _townDropDownList.Add(new DropDownListItem(string.Empty, "Please select a Town", string.Empty));

                    foreach (var m in _townList)
                    {
                        _townDropDownList.Add(new DropDownListItem(m.Id, m.Name, m.ProvinceId));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return _townList.OrderBy(c => c.Name).ToList();
        }

        public async Task<List<ModelTown>> GetTownsByProvice(string provinceId)
        {
            await GetTowns();

            try
            {
                return _townList.Where(c => c.ProvinceId == provinceId).OrderBy(c => c.Name).ToList();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return new List<ModelTown>();
        }

        public async Task<List<DropDownListItem>> GetDropDownTownsByProvice(string provinceId)
        {           
            if (_townDropDownList == null)
                _townDropDownList = new List<DropDownListItem>();

            if (_townDropDownList.Count == 0)            
                await GetTownsByProvice(provinceId);                
            
            return _townDropDownList.Where(c => string.IsNullOrWhiteSpace(c.ParentId) || c.ParentId == provinceId).ToList();
        }

        public async Task<List<ModelSuburb>> GetSuburbs()
        {
            await GetTowns();

            if (_suburbList == null)
                _suburbList = new List<ModelSuburb>();

            if (_suburbDropDownList == null)
                _suburbDropDownList = new List<DropDownListItem>();

            try
            {
                if (_suburbList.Count == 0)
                    _suburbList = await _apiDataService.GetSuburbs();

                if (_suburbDropDownList.Count == 0)
                {                   
                    _suburbDropDownList.Add(new DropDownListItem(string.Empty, "Please select a Suburb", string.Empty));

                    foreach (var m in _suburbList)
                    {
                        _suburbDropDownList.Add(new DropDownListItem(m.Id, m.Name, m.TownId));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return _suburbList.OrderBy(c => c.Name).ToList();
        }

        public async Task<List<ModelSuburb>> GetSuburbsByTown(string townId)
        {
            await GetSuburbs();

            try
            {
                return _suburbList.Where(c => c.TownId == townId).OrderBy(c => c.Name).ToList();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return new List<ModelSuburb>();
        }

        public async Task<List<DropDownListItem>> GetDropDownSuburbsByTown(string townId)
        {
            if (_suburbDropDownList == null)
                _suburbDropDownList = new List<DropDownListItem>();

            if (_suburbDropDownList.Count == 0)            
                await GetSuburbsByTown(townId);
                            
            return _suburbDropDownList.Where(c => string.IsNullOrWhiteSpace(c.ParentId) || c.ParentId == townId).ToList();
        }

        public async Task<List<ModelStreet>> GetStreets()
        {
            await GetSuburbs();

            if (_streetList == null)
                _streetList = new List<ModelStreet>();

            if (_streetDropDownList == null)
                _streetDropDownList = new List<DropDownListItem>();

            try
            {
                if (_streetList.Count == 0)
                    _streetList = await _apiDataService.GetStreets();

                if (_streetDropDownList.Count == 0)
                {                   
                    _streetDropDownList.Add(new DropDownListItem(string.Empty, "Please select a Street", string.Empty));

                    foreach (var m in _streetList)
                    {
                        _streetDropDownList.Add(new DropDownListItem(m.Id, m.Name, m.SuburbId));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return _streetList.OrderBy(c => c.Name).ToList();
        }

        public async Task<List<ModelStreet>> GetStreetsBySuburb(string suburbId)
        {
            await GetStreets();

            try
            {
                return _streetList.Where(c => c.SuburbId == suburbId).OrderBy(c => c.Name).ToList();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return new List<ModelStreet>();
        }

        public async Task<List<DropDownListItem>> GetDropDownStreetsBySuburb(string suburbId)
        {
            if(_streetDropDownList == null)
                _streetDropDownList = new List<DropDownListItem>();

            if (_streetDropDownList.Count == 0)            
                await GetStreetsBySuburb(suburbId);                              
            
            return _streetDropDownList.Where(c=> string.IsNullOrWhiteSpace(c.ParentId) || c.ParentId == suburbId).ToList();
        }

        #endregion
    }
}

