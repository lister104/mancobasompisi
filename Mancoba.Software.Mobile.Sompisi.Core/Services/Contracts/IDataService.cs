using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bluescore.DStv.ApiClient.Models;
using Bluescore.DStv.Core.ViewModels;
using Bluescore.DStv.Utils.Helpers;
using TechnitionTool.Mobile.Api.Shared.Models.Fleet;

namespace Bluescore.DStv.Core.Services.Contracts
{
	public interface IDataService
	{
		bool IsCheckedIn { get; set; }
		
		Task AcceptWorkOrder (string orderNumber);
		Task DeAcceptWorkOrder (IEnumerable orderIds);

		Task<AssetInstallSummaryViewModel> FetchAssetInstallSummary();

		Task<ModelAssetDetails> GetWorkOrderDetails (string workOrderId);

		Task<InstallSummaryModel> GetInstallSummary (string orgId, string assetId);

		Task<AssetModel> GetAssetSummary (string assetId);

		bool ForceFetch { get; set; }

		bool Connected { get; }

		Task<bool> CheckIn ();

		Task<bool> CheckOut ();

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

