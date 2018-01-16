using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Plugins.Sqlite;
using System.Collections.Generic;
using Mancoba.Sompisi.Data.Models;
using Mancoba.Sompisi.Data.LocalDb;
using Mancoba.Sompisi.Utils.Interfaces;
using Plugin.Connectivity;

namespace Mancoba.Sompisi.Data
{
    public interface IMancobaMobileDataApi
    {
        #region IMancobaAuthMicroService

        Task<bool> Ping();

        Task<ModelSystemUser> Login(string emailAddress, string password);

        Task<bool> Logout();

        Task<bool> CheckOut();

        Task<bool> CheckIn();

        Task<ModelSystemUser> GetSystemUser();

        Task<List<ModelProvider>> GetProviders();

        Task<ModelProvider> GetProvider(string id);

        Task<bool> FavouriteProvider(string id);

        Task<List<ModelInstaller>> GetInstallers();

        Task<ModelInstaller> GetInstaller(string id);

        Task<bool> FavouriteInstaller(string id);

        Task<List<ModelProviderProduct>> GetProviderProducts();

        Task<List<ModelProviderProduct>> GetProviderProducts(string providerId);

        Task<ModelProduct> GetProduct(string id);

        Task<List<ModelProduct>> GetProducts();

        Task<List<ModelProviderPayment>> GetProviderPaymentsByProvider(string providerId);

        Task<List<ModelProviderPayment>> GetProviderPaymentsByProduct(string productId);

        Task<List<ModelProviderPayment>> GetProviderPayments();

        Task<ModelProviderPayment> GetProviderPayment(string id);

        Task<bool> SaveApplication(ModelApplication application);

        Task<List<ModelApplication>> GetSentApplications();

        #endregion

        #region Addresses

        Task<List<ModelProvince>> GetProvinces();

        Task<ModelProvince> GetProvince(string id);

        Task<List<ModelTown>> GetTowns();

        Task<ModelTown> GetTown(string id);

        Task<List<ModelSuburb>> GetSuburbs();

        Task<ModelSuburb> GetSuburb(string id);

        Task<List<ModelStreet>> GetStreets();

        Task<ModelStreet> GetStreet(string id);

        #endregion
    }

    public sealed class MancobaMobileDataApi : IMancobaMobileDataApi
    {
        #region Variables

        private bool _isAuthenticated;
        private string _authenticatedMessage;
        private readonly IMvxSqliteConnectionFactory _connectionFactory;
        private readonly IPlatformCapabilities _platformCapabilities;
        private ModelSystemUser _systemUser;

        #endregion

        #region Constructors

	    public MancobaMobileDataApi(IMvxSqliteConnectionFactory connectionFactory, IPlatformCapabilities platformCapabilities)
	    {
		    Initialise();

		    if (connectionFactory != null)
		    {
			    _connectionFactory = connectionFactory;
			    TestData.ConnectionFactory = connectionFactory;
		    }

		    if (platformCapabilities != null)
		    {
			    _platformCapabilities = platformCapabilities;
			    TestData.PlatformCapabilities = platformCapabilities;
		    }
	    }

	    private void Initialise()
        {
            _systemUser = null;
            _isAuthenticated = false;
            _authenticatedMessage = null;
        }

        #endregion

        #region Properties

        public bool IsAuthenticated()
        {
            return !string.IsNullOrWhiteSpace(_authenticatedMessage) && _isAuthenticated;
        }

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

        public void SetAuthenticated(string message)
        {
            _authenticatedMessage = message;
        }

        #endregion

        #region SystemUser

        public async Task<ModelSystemUser> Login(string emailAddress, string password)
        {
	        TestData.ConnectionFactory = _connectionFactory;
	        TestData.PlatformCapabilities = _platformCapabilities;

			using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.Login(emailAddress, password);
                if (response == null)
                {
					response = await TestData.GetSystemUser(emailAddress, password);                                    
                }

                return _systemUser = response.ToModel();
            }
        }

        public async Task<bool> Logout()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.Logout(_systemUser.Id);
                return response > 0;
            }
        }

        public async Task<bool> Ping()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.Ping();
                return response > 0;
            }
        }

        public async Task<bool> CheckOut()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.CheckOut(_systemUser.Id);
                return response > 0;
            }
        }

        public async Task<bool> CheckIn()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.CheckIn(_systemUser.Id);
                return response > 0;
            }
        }

        public async Task<ModelSystemUser> GetSystemUser()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
	            if (_systemUser == null)
	            {
		            var response = await db.GetAnySystemUser();
					_systemUser = response.ToModel();
				}				

				return _systemUser;				
            }
        }

        #endregion

        #region Provider

        public async Task<List<ModelProvider>> GetProviders()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProviders();

				if (response == null || response.Count == 0)				
					response = await TestData.GetProviders();
				
				return response.ToModelCollection();
            }
        }

        public async Task<ModelProvider> GetProvider(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProvider(id);

                return response.ToModel();
            }
        }

        public async Task<bool> FavouriteProvider(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                await db.FavouriteProvider(id);

                return true;
            }
        }

        #endregion
		
        #region ProviderPayment

        public async Task<List<ModelProviderPayment>> GetProviderPaymentsByProvider(string providerId)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProviderPaymentsByProvider(providerId);

                return response.ToModelCollection();
            }
        }

        public async Task<List<ModelProviderPayment>> GetProviderPaymentsByProduct(string productId)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProviderPaymentsByProduct(productId);

                return response.ToModelCollection();
            }
        }

        public async Task<List<ModelProviderPayment>> GetProviderPayments()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProviderPayments();

                return response.ToModelCollection();
            }
        }

        public async Task<ModelProviderPayment> GetProviderPayment(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProviderPayment(id);

                return response.ToModel();
            }
        }

		#endregion

		#region Product

		public async Task<ModelProduct> GetProduct(string id)
		{
			using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
			{
				var response = await db.GetProduct(id);

				return response.ToModel();
			}
		}

		public async Task<List<ModelProduct>> GetProducts()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProducts();

				if (response == null || response.Count == 0)
					response = await TestData.GetProducts();

				return response.ToModelCollection();
            }
        }

        public async Task<List<ModelProviderProduct>> GetProviderProducts()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProviderProducts();

                return response.ToModelCollection();
            }
        }

        public async Task<ModelProviderProduct> GetProviderProduct(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProviderProduct(id);

                return response.ToModel();
            }
        }

        public async Task<List<ModelProviderProduct>> GetProviderProducts(string providerId)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProviderProducts(providerId);

                return response.ToModelCollection();
            }
        }

        #endregion

        #region Installer

        public async Task<List<ModelInstaller>> GetInstallers()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetInstallers();

				if (response == null || response.Count == 0)
					response = await TestData.GetInstallers();

				return response.ToModelCollection();
            }
        }

        public async Task<ModelInstaller> GetInstaller(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetInstaller(id);

                return response.ToModel();
            }
        }

        public async Task<bool> FavouriteInstaller(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                await db.FavouriteInstaller(id);

                return true;
            }
        }
        
        #endregion

        #region Addresses

        public async Task<ModelProvince> GetProvince(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProvince(id);
                return response.ToModel();
            }
        }

        public async Task<List<ModelProvince>> GetProvinces()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProvinces();

				if (response == null || response.Count == 0)
					response = await TestData.GetProvinces();

				return response.ToModelCollection();
            }
        }

        public async Task<ModelTown> GetTown(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetTown(id);
                return response.ToModel();
            }
        }

        public async Task<List<ModelTown>> GetTowns()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetTowns();

				if (response == null || response.Count == 0)
					response = await TestData.GetTowns();

				return response.ToModelCollection();
            }
        }

        public async Task<ModelSuburb> GetSuburb(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetSuburb(id);
                return response.ToModel();
            }
        }

        public async Task<List<ModelSuburb>> GetSuburbs()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetSuburbs();

				if (response == null || response.Count == 0)
					response = await TestData.GetSuburbs();

				return response.ToModelCollection();
            }
        }

        public async Task<ModelStreet> GetStreet(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetStreet(id);
                return response.ToModel();
            }
        }

        public async Task<List<ModelStreet>> GetStreets()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetStreets();

				if (response == null || response.Count == 0)
					response = await TestData.GetStreets();

				return response.ToModelCollection();
            }
        }

        #endregion

        #region Applications

        public async Task<bool> SaveApplication(ModelApplication application)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var entity = application.ToEntity();
                await db.SaveApplication(entity);
                return true;
            }
        }

        public async Task<ModelApplication> GetApplication(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetApplication(id);
                if (response == null)
                {
                    var tempTestList = await TestData.GetSentApplications().ConfigureAwait(false);
                    response = tempTestList.FirstOrDefault();
                }
                return response.ToModel();
            }
        }

        public async Task<List<ModelApplication>> GetSentApplications()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetSentApplications();

                if (response == null || response.Count == 0)
                    response = await TestData.GetSentApplications();

                return response.ToModelCollection();
            }
        }

        #endregion
    }
}

