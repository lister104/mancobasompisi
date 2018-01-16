using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bluescore.TV.Core.Language;
using Bluescore.TV.Core.Settings;
using Bluescore.TV.Core.ViewModels;
using Bluescore.TV.Data;
using Bluescore.TV.Data.Models;
using Bluescore.TV.Utils.Helpers;
using Bluescore.TV.Utils.Helpers.UserInteraction;
using MvvmCross.Platform;
using Plugin.Connectivity;

namespace Bluescore.TV.Core.Data
{
	public class MobileDataContext : IMobileDataContext
	{
		#region Singleton

		private static MobileDataContext _dataContextInstance;
		private static readonly object ObjectSync = new object();

		public static MobileDataContext DataContextInstance
		{
			get
			{
				if (_dataContextInstance == null)
				{
					lock (ObjectSync)
					{
						if (_dataContextInstance == null)
						{
							_dataContextInstance = new MobileDataContext();
							Task.Run(async () =>
							{
								try
								{
									var serverResult = await _dataContextInstance.Ping();
								}
								catch (Exception ex)
								{
								}

							});
						}
					}
				}
				return _dataContextInstance;
			}
		}

		#endregion

		#region Private Variables

		private IUserSettings _userSettings;
		private readonly IBluescoreMobileDataApi _mobileDataApi;

		#endregion

		#region Constructors

		public MobileDataContext()
		{
			_mobileDataApi = Mvx.Resolve<IBluescoreMobileDataApi>();
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
					return CrossConnectivity.Current.IsConnected;
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

		public async Task<bool> Login(string username, string password, string accessCode, bool isRetry = false)
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.NoConnectivity);
				return false;
			}

			try
			{
				var response = await _mobileDataApi.Login(username, password, accessCode);

				if (!isRetry && response.IsSuccessful)
				{
					var userSettings = Mvx.Resolve<IUserSettings>();
					userSettings.Username = username;
					userSettings.Password = password;
				}

				return response.IsSuccessful;
			}
			catch (Exception ex)
			{
				Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.LoginErrorTitle);
			}

			return false;
		}

		public async Task<bool> TryAutoLogin()
		{
			var userSettings = Mvx.Resolve<IUserSettings>();
			return await Login(userSettings.Username, userSettings.Password, userSettings.AccessCode);

		}

		#endregion

		#region Customers	
		private List<CustomerItemViewModel> _customerItemList;

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

		public async Task<ModelCustomer> GetCustomerById(string id)
		{			
			try
			{
				return await _mobileDataApi.GetCustomerById(id);
			}
			catch (Exception ex)
			{
				ErrorHandler.HandleError(ex);				
			}

			return null;
		}

		public async Task<bool> FindCustomers(string searchText)
		{			
			try
			{
				var data = await _mobileDataApi.FindCustomers(searchText);
				_customerItemList = Mapper.Map<List<ModelCustomer>, List<CustomerItemViewModel>>(data);
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

		public async Task<bool> SaveCustomer(ModelCustomer customer)
		{			
			try
			{				
				return await _mobileDataApi.SaveCustomer(customer);

			}
			catch (Exception ex)
			{
				ErrorHandler.HandleError(ex);
				Mvx.Resolve<IUserInteraction>().ToastErrorAlert(ex.ToString());
			}

			return false;
		}

		#endregion

		#region Assets	
		private List<ModelAsset> _assets;

		public async Task<List<ModelAsset>> GetAssets(string customerId, string accoundId)
		{
			try
			{
				if(_assets == null)
					_assets = new List<ModelAsset>();

				if (_assets.Count > 0)
				{
					var resp = (from e in _assets where e.CustomerId == customerId || e.AccountId == accoundId select e).Distinct().ToList();

					if (resp.Count > 0)
						return resp;
				}

				var list = await _mobileDataApi.GetAssets(customerId, accoundId);

				if (list.Count > 0)
					_assets.AddRange(list);

				return list;
			}
			catch (Exception ex)
			{
				ErrorHandler.HandleError(ex);
			}

			return null;
		}		
	
		public async Task<bool> SaveAssets(List<ModelAsset> assets)
		{
			try
			{
				return await _mobileDataApi.SaveCustomer(customer);

			}
			catch (Exception ex)
			{
				ErrorHandler.HandleError(ex);
				Mvx.Resolve<IUserInteraction>().ToastErrorAlert(ex.ToString());
			}

			return false;
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

	}
}
