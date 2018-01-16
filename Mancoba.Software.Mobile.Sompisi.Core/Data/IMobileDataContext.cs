using System.Collections.Generic;
using System.Threading.Tasks;
using Bluescore.TV.Core.ViewModels;
using Bluescore.TV.Data.Models;
using Bluescore.TV.Utils.Helpers;

namespace Bluescore.TV.Core.Data
{
	public interface IMobileDataContext
	{
		#region Properties
		bool IsCheckedIn { get; set; }

		bool ForceFetch { get; set; }

		bool Connected { get; }

		bool IsInternetConnected { get; }
		#endregion

		#region Auth	Task<bool> Ping();
		Task<bool> Login(string username, string password, string accessCode);
		Task<bool> TryAutoLogin();

		Task<bool> CheckIn();

		Task<bool> CheckOut();
		#endregion

		#region Assets
		#endregion

		Task<AssetInstallSummaryViewModel> FetchAssetInstallSummary();

		Task<ModelAssetDetails> GetWorkOrderDetails(string workOrderId);

		Task<InstallSummaryModel> GetInstallSummary(string orgId, string assetId);

		Task<AssetModel> GetAssetSummary(string assetId);

	

		#region Customers	

		bool HasFoundCustomers();

		void ClearCustomerList();

		Task<ModelCustomerDetails> GetCustomerDetails(string id);

		Task<bool> FindCustomers(string searchText);

		Task<List<CustomerItemViewModel>> GetFoundCustomers();

		Task<bool> SaveCustomer(ModelCustomerDetails customer);

		#endregion

		#region Addresses

		Task<List<ModelProvince>> GetProvinces();
		Task<List<ModelTown>> GetTowns();
		Task<List<ModelTown>> GetTownsByProvice(string provinceId);
		Task<List<ModelSuburb>> GetSuburbs();
		Task<List<ModelSuburb>> GetSuburbsByTown(string townId);
		Task<List<ModelStreet>> GetStreets();
		Task<List<ModelStreet>> GetStreetsBySuburb(string suburbId);
		Task<List<DropDownListItem>> GetDropDownProvinces();
		Task<List<DropDownListItem>> GetDropDownTownsByProvice(string provinceId);
		Task<List<DropDownListItem>> GetDropDownSuburbsByTown(string townId);
		Task<List<DropDownListItem>> GetDropDownStreetsBySuburb(string suburbId);

		#endregion
	}
}
