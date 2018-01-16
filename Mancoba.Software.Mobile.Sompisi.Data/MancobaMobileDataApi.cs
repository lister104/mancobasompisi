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

        /// <summary>
        /// Initializes a new instance of the <see cref="MancobaMobileDataApi"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="platformCapabilities">The platform capabilities.</param>
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

        /// <summary>
        /// Initialises this instance.
        /// </summary>
        private void Initialise()
        {
            _systemUser = null;
            _isAuthenticated = false;
            _authenticatedMessage = null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Determines whether this instance is authenticated.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAuthenticated()
        {
            return !string.IsNullOrWhiteSpace(_authenticatedMessage) && _isAuthenticated;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is internet connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is internet connected; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Sets the authenticated.
        /// </summary>
        /// <param name="message">The message.</param>
        public void SetAuthenticated(string message)
        {
            _authenticatedMessage = message;
        }

        #endregion

        #region SystemUser

        /// <summary>
        /// Logins the specified email address.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Logouts this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Logout()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.Logout(_systemUser.Id);
                return response > 0;
            }
        }

        /// <summary>
        /// Pings this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Ping()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.Ping();
                return response > 0;
            }
        }

        /// <summary>
        /// Checks the out.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckOut()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.CheckOut(_systemUser.Id);
                return response > 0;
            }
        }

        /// <summary>
        /// Checks the in.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckIn()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.CheckIn(_systemUser.Id);
                return response > 0;
            }
        }

        /// <summary>
        /// Gets the system user.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ModelProvider> GetProvider(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProvider(id);

                return response.ToModel();
            }
        }

        /// <summary>
        /// Favourites the provider.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the provider payments by provider.
        /// </summary>
        /// <param name="providerId">The provider identifier.</param>
        /// <returns></returns>
        public async Task<List<ModelProviderPayment>> GetProviderPaymentsByProvider(string providerId)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProviderPaymentsByProvider(providerId);

                return response.ToModelCollection();
            }
        }

        /// <summary>
        /// Gets the provider payments by product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        public async Task<List<ModelProviderPayment>> GetProviderPaymentsByProduct(string productId)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProviderPaymentsByProduct(productId);

                return response.ToModelCollection();
            }
        }

        /// <summary>
        /// Gets the provider payments.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ModelProviderPayment>> GetProviderPayments()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProviderPayments();

                return response.ToModelCollection();
            }
        }

        /// <summary>
        /// Gets the provider payment.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ModelProduct> GetProduct(string id)
		{
			using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
			{
				var response = await db.GetProduct(id);

				return response.ToModel();
			}
		}

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the provider products.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ModelProviderProduct>> GetProviderProducts()
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProviderProducts();

                return response.ToModelCollection();
            }
        }

        /// <summary>
        /// Gets the provider product.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ModelProviderProduct> GetProviderProduct(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProviderProduct(id);

                return response.ToModel();
            }
        }

        /// <summary>
        /// Gets the provider products.
        /// </summary>
        /// <param name="providerId">The provider identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the installers.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the installer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ModelInstaller> GetInstaller(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetInstaller(id);

                return response.ToModel();
            }
        }

        /// <summary>
        /// Favourites the installer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the province.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ModelProvince> GetProvince(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetProvince(id);
                return response.ToModel();
            }
        }

        /// <summary>
        /// Gets the provinces.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the town.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ModelTown> GetTown(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetTown(id);
                return response.ToModel();
            }
        }

        /// <summary>
        /// Gets the towns.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the suburb.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ModelSuburb> GetSuburb(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetSuburb(id);
                return response.ToModel();
            }
        }

        /// <summary>
        /// Gets the suburbs.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the street.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ModelStreet> GetStreet(string id)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var response = await db.GetStreet(id);
                return response.ToModel();
            }
        }

        /// <summary>
        /// Gets the streets.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Saves the application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns></returns>
        public async Task<bool> SaveApplication(ModelApplication application)
        {
            using (var db = new MancobaLocalDataApi(_connectionFactory, _platformCapabilities))
            {
                var entity = application.ToEntity();
                await db.SaveApplication(entity);
                return true;
            }
        }

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the sent applications.
        /// </summary>
        /// <returns></returns>
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

