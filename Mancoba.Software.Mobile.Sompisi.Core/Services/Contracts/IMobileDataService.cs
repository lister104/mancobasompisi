using System.Collections.Generic;
using System.Threading.Tasks;
using Mancoba.Sompisi.Core.ViewModels;
using Mancoba.Sompisi.Data.Models;
using Mancoba.Sompisi.Utils.Helpers;

namespace Mancoba.Sompisi.Core.Services.Contracts
{
	public interface IMobileDataService
	{
		#region Properties

		bool IsCheckedIn { get; set; }

		bool ForceFetch { get; set; }

		bool Connected { get; }

		bool IsInternetConnected { get; }

        #endregion

        #region Auth	

        Task<bool> Ping();

		Task<bool> Login(string username, string password, bool isRetry = false);

		Task<bool> TryAutoLogin();

        Task<bool> SaveSystemUser(ModelSystemUser user);

        Task<bool> CheckIn();

		Task<ModelSystemUser> GetSystemUser();

		Task<bool> CheckOut();

        #endregion
       
        #region Provider

        Task<ModelProvider> GetProvider(string id);

        Task<List<ModelProvider>> GetProviders();

        Task<List<ModelProvider>> FindProviders(string searchText);

        Task<bool> FavouriteProvider(string id);

        #endregion

        #region Installer

        Task<ModelInstaller> GetInstaller(string id);

        Task<List<ModelInstaller>> GetInstallers();

        Task<List<ModelInstaller>> FindInstallers(string searchText);

        Task<bool> FavouriteInstaller(string id);

        #endregion      

        #region Product

        Task<ModelProduct> GetProduct(string id);

        Task<List<ModelProduct>> GetProducts();

        Task<ModelProviderProduct> GetProviderProduct(string id);

        Task<List<ModelProviderProduct>> GetProviderProducts();

        #endregion

        #region Addresses

        Task<List<ModelProvince>> GetProvinces();

        Task<ModelProvince> GetProvince(string id);

        Task<List<ModelTown>> GetTowns();

        Task<ModelTown> GetTown(string id);

        Task<List<ModelTown>> GetTownsByProvice(string provinceId);

		Task<List<ModelSuburb>> GetSuburbs();

        Task<ModelSuburb> GetSuburb(string id);

        Task<List<ModelSuburb>> GetSuburbsByTown(string townId);

		Task<List<ModelStreet>> GetStreets();

        Task<ModelStreet> GetStreet(string id);

        Task<List<ModelStreet>> GetStreetsBySuburb(string suburbId);
        
        Task<List<DropDownListItem>> GetDropDownProvinces();

		Task<List<DropDownListItem>> GetDropDownTownsByProvice(string provinceId);

		Task<List<DropDownListItem>> GetDropDownSuburbsByTown(string townId);

		Task<List<DropDownListItem>> GetDropDownStreetsBySuburb(string suburbId);

        #endregion

        #region Application

        Task<bool> SaveApplication(ModelApplication application);

        Task<List<ModelApplication>> GetSentApplications();

        Task<ModelApplication> GetSentApplication(string id);

        Task<List<ModelApplication>> FindApplications(string searchText);

        #endregion
    }
}
