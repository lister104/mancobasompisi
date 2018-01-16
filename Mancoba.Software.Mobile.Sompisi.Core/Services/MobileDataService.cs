using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mancoba.Sompisi.Core.Services.Contracts;
using Mancoba.Sompisi.Data;
using Mancoba.Sompisi.Data.Models;
using Mancoba.Sompisi.Utils.Helpers;
using Mancoba.Sompisi.Utils.Helpers.UserInteraction;
using Mancoba.Sompisi.Utils.Interfaces;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Platform;
using Plugin.Connectivity;

namespace Mancoba.Sompisi.Core.Services
{
	public class MobileDataService : IMobileDataService
	{
		#region Singleton

		private static IMobileDataService _dataServiceInstance;
		private static readonly object ObjectSync = new object();

		public static IMobileDataService DataServiceInstance
		{
			get
			{
				if (_dataServiceInstance == null)
				{
					lock (ObjectSync)
					{
						if (_dataServiceInstance == null)
						{
							_dataServiceInstance = new MobileDataService();
							Task.Run(async () =>
							{
								try
								{
									var serverResult = await _dataServiceInstance.Ping();
								}
								catch (Exception ex)
								{
								}

							});
						}
					}
				}
				return _dataServiceInstance;
			}
		}

		#endregion

		#region Private Variables

		//private IUserSettings _userSettings;
		private readonly IMancobaMobileDataApi _mobileDataApi;

		#endregion

		#region Constructors

		public MobileDataService()
		{
           // _userSettings = Mvx.Resolve<IUserSettings>();
            _mobileDataApi = Mvx.Resolve<IMancobaMobileDataApi>();
		}		

		#endregion

		#region Properties

		public bool IsCheckedIn { get; set; }

		public bool ForceFetch { get; set; }

		public bool Connected { get; }

		public bool IsInternetConnected
		{
			get
			{
				try
				{
					return true;
					//return CrossConnectivity.Current.IsConnected;
				}
				catch (Exception)
				{
					return false;
				}
			}
		}

		#endregion

		#region Auth

		public async Task<bool> Ping()
		{			
			try
			{
				return await _mobileDataApi.Ping();
			}
			catch (Exception ex)
			{
				Mvx.Resolve<IUserInteraction>().ToastErrorAlert(ex);
			}

			return false;
		}

		public async Task<bool> Login(string username, string password, bool isRetry = false)
		{
			if (!IsInternetConnected)
			{
				Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.NoConnectivity);
				return false;
			}

			try
			{
				var response = await _mobileDataApi.Login(username, password);

				if (!isRetry && response.IsSuccessful)
				{
					var userSettings = Mvx.Resolve<IUserSettings>();
					userSettings.Username = username;
					userSettings.Password = password;
				    userSettings.AccessCode = string.Empty; // accessCode;
                }

				return response.IsSuccessful;
			}
			catch (Exception ex)
			{
				Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.LoginErrorTitle);
			}

			return false;
		}

        public async Task<bool> CheckIn()
        {
            try
            {
                return await _mobileDataApi.CheckIn();
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IUserInteraction>().ToastErrorAlert(ex);
            }

            return false;
        }

        public async Task<bool> CheckOut()
        {
            try
            {
                return await _mobileDataApi.CheckOut();
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IUserInteraction>().ToastErrorAlert(ex);
            }

            return false;
        }

        public async Task<bool> TryAutoLogin()
		{
			var userSettings = Mvx.Resolve<IUserSettings>();
			return await Login(userSettings.Username, userSettings.Password, true);

		}

	    public async Task<bool> SaveSystemUser(ModelSystemUser user)
	    {
	        return await Task.Run(() => true);
	    }

		public async Task<ModelSystemUser> GetSystemUser()
		{
			return await _mobileDataApi.GetSystemUser();
		}

		#endregion

		#region Provider	

		private List<ModelProvider> _providers;                       

		public async Task<ModelProvider> GetProvider(string id)
		{
		    await GetProviders();

		    return await Task.Run(() =>
		    {

		        try
		        {
                    if (_providers != null && _providers.Count > 0)
                        return _providers.FirstOrDefault(c => c.Id == id);
                }
		        catch (Exception ex)
		        {
		            ErrorHandler.HandleError(ex);
		        }

		        return null;
		    });
		}

        public async Task<List<ModelProvider>> GetProviders()
        {
            try
            {
                if(_providers == null || _providers.Count == 0)
                    _providers = await _mobileDataApi.GetProviders();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return _providers;
        }

	    public async Task<List<ModelProvider>> FindProviders(string searchText)
		{
            await GetProviders();

            try
            {
                return _providers.Where(c => c.Name.Trim().ToLower().Contains(searchText.ToLower().Trim())).ToList();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
            finally
            {
                ForceFetch = false;
            }

            return new List<ModelProvider>();
        }

        public async Task<bool> FavouriteProvider(string id)
	    {
            try
            {
                if (_providers != null && _providers.Count > 0)
                {
                    var prov = _providers.FirstOrDefault(c => c.Id == id);
                    if (prov != null)
                    {
                        prov.IsFavourite = !prov.IsFavourite;
                        _providers[_providers.IndexOf(prov)] = prov;
                    }

                }

                return await _mobileDataApi.FavouriteProvider(id);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return true;
        }

        #endregion

        #region Installer	
        
        private List<ModelInstaller> _installers;        

        public async Task<ModelInstaller> GetInstaller(string id)
        {
            await GetInstallers();
            try
            {
                if(_installers != null && _installers.Count > 0)
                   return _installers.FirstOrDefault(c=>c.Id == id);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return null;
        }

        public async Task<List<ModelInstaller>> GetInstallers()
        {
            try
            {
                if (_installers == null || _installers.Count == 0)
                    _installers =  await _mobileDataApi.GetInstallers();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return _installers;
        }

        public async Task<bool> FavouriteInstaller(string id)
        {
            try
            {
                if (_installers != null && _installers.Count > 0)
                {
                    var prov = _installers.FirstOrDefault(c => c.Id == id);
                    if (prov != null)
                    {
                        prov.IsFavourite = !prov.IsFavourite;
                        _installers[_installers.IndexOf(prov)] = prov;
                    }

                }

                return await _mobileDataApi.FavouriteInstaller(id);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return true;
        }

        public async Task<List<ModelInstaller>> FindInstallers(string searchText)
        {
            await GetInstallers();

            try
            {                
                return _installers.Where(c => c.Name.Trim().ToLower().Contains(searchText.ToLower().Trim())).ToList();               
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
            finally
            {
                ForceFetch = false;
            }

            return new List<ModelInstaller>();
        }
        
        #endregion

        #region Product	

        private List<ModelProviderProduct> _providerProducts;
        private List<ModelProduct> _products;

	    public async Task<ModelProduct> GetProduct(string id)
	    {
	        await GetProducts();

	        try
	        {
	            if (_products != null && _products.Count > 0)
	                return _products.FirstOrDefault(c => c.Id == id);
	        }
	        catch (Exception ex)
	        {
	            ErrorHandler.HandleError(ex);
	        }

	        return null;
	    }

	    public async Task<List<ModelProduct>> GetProducts()
		{
			try
			{
				if(_products == null || _products.Count == 0)
                    _products = await _mobileDataApi.GetProducts();
			}
			catch (Exception ex)
			{
				ErrorHandler.HandleError(ex);
			}

			return _products;
		}

        public async Task<List<ModelProviderProduct>> GetProviderProducts()
        {
            try
            {
                if (_providerProducts == null || _providerProducts.Count == 0)
                    _providerProducts = await _mobileDataApi.GetProviderProducts();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return _providerProducts;
        }

        public async Task<ModelProviderProduct> GetProviderProduct(string id)
        {
            await GetProviderProducts();

            return await Task.Run(() =>
            {
                try
                {
                    if (_providerProducts != null && _providerProducts.Count > 0)
                        return _providerProducts.FirstOrDefault(c => c.Id == id);
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleError(ex);
                }

                return null;
            });
        }

        public async Task<List<ModelProviderProduct>> GetProviderProducts(string providerId)
	    {
	        await GetProviderProducts();

	        return await  Task.Run(() =>
	        {
	            try
	            {

	                if (_providerProducts != null && _providerProducts.Count > 0)
	                    return _providerProducts.Where(c => c.ProviderId == providerId).ToList();
	            }
	            catch (Exception ex)
	            {
	                ErrorHandler.HandleError(ex);
	            }

	            return new List<ModelProviderProduct>();
	        });
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
					_provinceList = await _mobileDataApi.GetProvinces();

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

        public async Task<ModelProvince> GetProvince(string id)
        {
            try
            {
                if (_provinceList == null)
                    _provinceList = new List<ModelProvince>();

                if (_provinceList.Count > 0)
                {
                    var resp = (from e in _provinceList where e.Id == id select e).FirstOrDefault();

                    if (resp != null)
                        return resp;
                }

                var model = await _mobileDataApi.GetProvince(id);

                if (model != null)
                    _provinceList.Add(model);

                return model;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return null;
        }

        public async Task<List<DropDownListItem>> GetDropDownProvinces()
		{
			if (_provinceDropDownList == null)
				_provinceDropDownList = new List<DropDownListItem>();

			if (_provinceDropDownList.Count == 0)
				await GetProvinces();

			return _provinceDropDownList;
		}

        public async Task<ModelTown> GetTown(string id)
        {
            try
            {
                if (_townList == null)
                    _townList = new List<ModelTown>();

                if (_townList.Count > 0)
                {
                    var resp = (from e in _townList where e.Id == id select e).FirstOrDefault();

                    if (resp != null)
                        return resp;
                }

                var model = await _mobileDataApi.GetTown(id);

                if (model != null)
                    _townList.Add(model);

                return model;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return null;
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
					_townList = await _mobileDataApi.GetTowns();

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

        public async Task<ModelSuburb> GetSuburb(string id)
        {
            try
            {
                if (_suburbList == null)
                    _suburbList = new List<ModelSuburb>();

                if (_suburbList.Count > 0)
                {
                    var resp = (from e in _suburbList where e.Id == id select e).FirstOrDefault();

                    if (resp != null)
                        return resp;
                }

                var model = await _mobileDataApi.GetSuburb(id);

                if (model != null)
                    _suburbList.Add(model);

                return model;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return null;
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
					_suburbList = await _mobileDataApi.GetSuburbs();

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

	    public async Task<ModelStreet> GetStreet(string id)
	    {
            try
            {
                if (_streetList == null)
                    _streetList = new List<ModelStreet>();

                if (_streetList.Count > 0)
                {
                    var resp = (from e in _streetList where e.Id == id select e).FirstOrDefault();

                    if (resp != null)
                        return resp;
                }

                var model = await _mobileDataApi.GetStreet(id);

                if (model != null)
                    _streetList.Add(model);

                return model;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return null;
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
					_streetList = await _mobileDataApi.GetStreets();

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
			if (_streetDropDownList == null)
				_streetDropDownList = new List<DropDownListItem>();

			if (_streetDropDownList.Count == 0)
				await GetStreetsBySuburb(suburbId);

			return _streetDropDownList.Where(c => string.IsNullOrWhiteSpace(c.ParentId) || c.ParentId == suburbId).ToList();
		}

        #endregion

        #region Application

        private List<ModelApplication> _applications;

        public async Task<bool> SaveApplication(ModelApplication application)
        {
            try
            {
                return await _mobileDataApi.SaveApplication(application);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return true;
        }

        public async Task<List<ModelApplication>> GetSentApplications()
        {
            try
            {
                if (_applications == null || _applications.Count == 0)
                    _applications = await _mobileDataApi.GetSentApplications();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }

            return _applications;
        }

        public async Task<ModelApplication> GetSentApplication(string id)
        {
            await GetSentApplications();
            try
            {
                if (_applications != null && _applications.Count > 0)
                    return _applications.FirstOrDefault(c => c.Id == id);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
            return null;
        }

        public async Task<List<ModelApplication>> FindApplications(string searchText)
        {
            await GetSentApplications();

            try
            {
                return _applications.Where(c => c.FirstName.Trim().ToLower().Contains(searchText.ToLower().Trim())).ToList();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
            finally
            {
                ForceFetch = false;
            }

            return new List<ModelApplication>();
        }

        #endregion
    }
}
