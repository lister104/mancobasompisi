using System;
using System.Collections.Async;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mancoba.Sompisi.Data.LocalDb.Entities;
using Mancoba.Sompisi.Utils.Helpers;
using Mancoba.Sompisi.Utils.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Plugins.Sqlite;
using SQLite;

namespace Mancoba.Sompisi.Data.LocalDb
{
    internal class MancobaLocalDataApi : IDisposable
    {
	    private const string SqlDbName = "Mancoba.Sompisi.db3";
		private SQLiteAsyncConnection _connection;
        private SQLiteConnection _syncConnection;
        private readonly IMvxSqliteConnectionFactory _connectionFactory;
        private readonly IPlatformCapabilities _platformCapabilities;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MancobaLocalDataApi"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="platformCapabilities">The platform capabilities.</param>
        public MancobaLocalDataApi(IMvxSqliteConnectionFactory connectionFactory, IPlatformCapabilities platformCapabilities)
        {
            //#warning If anything changes in here, DO HARDWARE RESET ON iOS SIMULATOR        
            _connectionFactory = connectionFactory;
            _platformCapabilities = platformCapabilities;
           
            CreateTables();
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns></returns>
        private SQLiteAsyncConnection GetConnection()
        {
            if(_connection == null)
                _connection = _connectionFactory.GetAsyncConnection(SqlDbName);

            return _connection;
        }

        /// <summary>
        /// Gets the synchronize connection.
        /// </summary>
        /// <returns></returns>
        private SQLiteConnection GetSyncConnection()
        {
            if (_syncConnection == null)
                _syncConnection = _connectionFactory.GetConnection(SqlDbName);

            return _syncConnection;
        }

        #endregion

        #region IDisposable

        private bool _disposed = false;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        public void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				try
				{
					if (_connection != null)
						_connection.DisposeIfDisposable();                    
                }
				catch { }

                try
                {
                    if (_syncConnection != null)
                        _syncConnection.DisposeIfDisposable();

                    if (_syncConnection != null)
                        _syncConnection.Dispose();
                }
                catch { }

                try
				{
					if (_connectionFactory != null)
						_connectionFactory.DisposeIfDisposable();
				}
				catch { }
			}

			// Free any unmanaged objects here.
			//
			_disposed = true;
		}

        /// <summary>
        /// Finalizes an instance of the <see cref="MancobaLocalDataApi"/> class.
        /// </summary>
        ~MancobaLocalDataApi()
		{
			Dispose(false);
		}

		#endregion

        #region Tables

        private static bool _hasCreatedTables;
        private static readonly object TableSync = new object();

        /// <summary>
        /// Creates the tables.
        /// </summary>
        private void CreateTables()
        {
            if (_hasCreatedTables)
                return;

            if (Mvx.Resolve<IUserSettings>().HasTablesCreated)
                return;

            DropTables();

            lock (TableSync)
            {               
                //User
                GetSyncConnection().CreateTable<EntitySystemUser>();

                //Address
                GetSyncConnection().CreateTable<EntityProvince>();
                GetSyncConnection().CreateTable<EntityTown>();
                GetSyncConnection().CreateTable<EntitySuburb>();
                GetSyncConnection().CreateTable<EntityStreet>();               

                //Customer
                GetSyncConnection().CreateTable<EntityInstaller>();
                GetSyncConnection().CreateTable<EntityProvider>();
                GetSyncConnection().CreateTable<EntityProduct>();
                GetSyncConnection().CreateTable<EntityProviderProduct>();
                GetSyncConnection().CreateTable<EntityProviderPayment>();

                //Applications
                GetSyncConnection().CreateTable<EntityApplication>();

                Mvx.Resolve<IUserSettings>().HasTablesCreated = true;
                _hasCreatedTables = true;
            }
        }

        /// <summary>
        /// Drops the tables.
        /// </summary>
        private void DropTables()
        {
            lock (TableSync)
            {
                try
                {
                    //User
                    GetSyncConnection().DropTable<EntitySystemUser>();

                    //Address
                    GetSyncConnection().DropTable<EntityProvince>();
                    GetSyncConnection().DropTable<EntityTown>();
                    GetSyncConnection().DropTable<EntitySuburb>();
                    GetSyncConnection().DropTable<EntityStreet>();

                    //Account
                    GetSyncConnection().DropTable<EntityInstaller>();
                    GetSyncConnection().DropTable<EntityProvider>();
                    GetSyncConnection().DropTable<EntityProduct>();
                    GetSyncConnection().DropTable<EntityProviderProduct>();
                    GetSyncConnection().DropTable<EntityProviderPayment>();

                    //Application
                    GetSyncConnection().DropTable<EntityApplication>();
                }
                catch (Exception)
                {

                }
            }
        }

        #endregion

        #region Auth

        /// <summary>
        /// Logins the specified email address.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<EntitySystemUser> Login(string emailAddress, string password)
	    {			
			return await Util.TryAsync(() => GetConnection().Table<EntitySystemUser>().FirstOrDefaultAsync(),
				  failureMessage: "DataContext failed to find a EntitySystemUser");
		}

        /// <summary>
        /// Logouts the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<int> Logout(string userId)
	    {		    
            return await Task.Run(()=>1);
		}

        /// <summary>
        /// Pings this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<int> Ping()
	    {
            return await Task.Run(() => 1);
        }

        /// <summary>
        /// Checks the in.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<int> CheckIn(string userId)
		{
            return await Task.Run(() => 1);
        }

        /// <summary>
        /// Checks the out.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<int> CheckOut(string userId)
		{
            return await Task.Run(() => 1);
        }

        #endregion

        #region Users

        /// <summary>
        /// Gets the system user by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<EntitySystemUser> GetSystemUserById(string id)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntitySystemUser>().Where(o => o.Id== id).FirstOrDefaultAsync(),
			   failureMessage: "DataContext failed to load EntitySystemUser");
		}

        /// <summary>
        /// Gets any system user.
        /// </summary>
        /// <returns></returns>
        public async Task<EntitySystemUser> GetAnySystemUser()
		{
			return await Util.TryAsync(() => GetConnection().Table<EntitySystemUser>().FirstOrDefaultAsync(),
			   failureMessage: "DataContext failed to load GetAnySystemUser");
		}

        /// <summary>
        /// Saves the system user.
        /// </summary>
        /// <param name="town">The town.</param>
        /// <returns></returns>
        public async Task<int> SaveSystemUser(EntitySystemUser town)
		{
			return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(town),
			   failureMessage: "DataContext failed to SAVE EntitySystemUser");
		}

        #endregion

        #region Installer

        /// <summary>
        /// Gets the installer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<EntityInstaller> GetInstaller(string id)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityInstaller>().Where(o => o.Id == id).FirstOrDefaultAsync(),
			   failureMessage: "DataContext failed to load EntityInstaller");
		}

        /// <summary>
        /// Favourites the installer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<int> FavouriteInstaller(string id)
        {
            var entity =  await GetInstaller(id);          
			if (entity != null)
			{
				entity.IsFavourite = !entity.IsFavourite;
				return await SaveInstaller(entity);
			}
			return 1;
		}

        /// <summary>
        /// Gets the installers.
        /// </summary>
        /// <returns></returns>
        public async Task<List<EntityInstaller>> GetInstallers()
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityInstaller>().ToListAsync(),
               failureMessage: "DataContext failed to load EntityInstaller");
        }

        /// <summary>
        /// Saves the installer.
        /// </summary>
        /// <param name="installer">The installer.</param>
        /// <returns></returns>
        public async Task<int> SaveInstaller(EntityInstaller installer)
		{
            installer.Id = UtilsService.GenerateId(installer.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(installer),
			  failureMessage: "DataContext failed to SAVE EntityInstaller");
		}

        /// <summary>
        /// Saves the installer.
        /// </summary>
        /// <param name="customers">The customers.</param>
        /// <returns></returns>
        public async Task<int> SaveInstaller(List<EntityInstaller> customers)
		{
			await customers.ParallelForEachAsync(async customer =>
			{
                customer.Id = UtilsService.GenerateId(customer.Id);
                await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(customer),
					failureMessage: "DataContext failed to SAVE EntityCustomers");
			});

			return await Task.Run(() => 1);
		}

        #endregion

        #region Product

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<EntityProduct> GetProduct(string id)
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityProduct>().Where(o => o.Id == id).FirstOrDefaultAsync());
        }

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <returns></returns>
        public async Task<List<EntityProduct>> GetProducts()
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityProduct>().ToListAsync());
        }

        /// <summary>
        /// Gets the provider product.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<EntityProviderProduct> GetProviderProduct(string id)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityProviderProduct>().Where(o => o.Id == id).FirstOrDefaultAsync());
		}

        /// <summary>
        /// Gets the provider products.
        /// </summary>
        /// <param name="providerId">The provider identifier.</param>
        /// <returns></returns>
        public async Task<List<EntityProviderProduct>> GetProviderProducts(string providerId)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityProviderProduct>().Where(o => o.ProviderId == providerId).ToListAsync());
		}

        /// <summary>
        /// Gets the provider products.
        /// </summary>
        /// <returns></returns>
        public async Task<List<EntityProviderProduct>> GetProviderProducts()
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityProviderProduct>().ToListAsync());
        }

        /// <summary>
        /// Saves the provider product.
        /// </summary>
        /// <param name="providerProduct">The provider product.</param>
        /// <returns></returns>
        public async Task<int> SaveProviderProduct(EntityProviderProduct providerProduct)
		{
            providerProduct.Id = UtilsService.GenerateId(providerProduct.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(providerProduct),
			  failureMessage: "DataContext failed to SAVE EntityProviderProduct");
		}

        /// <summary>
        /// Saves the product.
        /// </summary>
        /// <param name="products">The products.</param>
        /// <returns></returns>
        public async Task<bool> SaveProduct(List<EntityProduct> products)
	    {
		    await products.ParallelForEachAsync(async asset =>
		    {
                asset.Id = UtilsService.GenerateId(asset.Id);

                await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(asset),
				    failureMessage: "DataContext failed to SAVE EntityProduct");
		    });

		    return await Task.Run(() => true);
	    }

        /// <summary>
        /// Saves the provider product.
        /// </summary>
        /// <param name="products">The products.</param>
        /// <returns></returns>
        public async Task<bool> SaveProviderProduct(List<EntityProviderProduct> products)
		{
			await products.ParallelForEachAsync(async asset =>
			{
				asset.Id = UtilsService.GenerateId(asset.Id);

				await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(asset),
					failureMessage: "DataContext failed to SAVE EntityProviderProduct");
			});

			return await Task.Run(() => true);
		}

        #endregion

        #region Provider

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<EntityProvider> GetProvider(string id)
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityProvider>().OrderBy(o => o.Id == id).FirstOrDefaultAsync(),
               failureMessage: "DataContext failed to load EntityProvider by GetAccount");
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <returns></returns>
        public async Task<List<EntityProvider>> GetProviders()
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityProvider>().ToListAsync(),
               failureMessage: "DataContext failed to load EntityProvider");
        }

        /// <summary>
        /// Finds the providers.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public async Task<List<EntityProvider>> FindProviders(string searchText)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityProvider>().Where(o => o.Name == searchText).ToListAsync(),
			   failureMessage: "DataContext failed to load EntityProvider");
		}

        /// <summary>
        /// Favourites the provider.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<int> FavouriteProvider(string id)
        {
            var entity = await GetProvider(id);
	        if (entity != null)
	        {
		        entity.IsFavourite = !entity.IsFavourite;
		        return await SaveProvider(entity);
	        }
	        return 1;
        }

        /// <summary>
        /// Saves the provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public async Task<int> SaveProvider(EntityProvider provider)
		{
            provider.Id = UtilsService.GenerateId(provider.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(provider),
			   failureMessage: "DataContext failed to SAVE EntityProvider");
		}

        /// <summary>
        /// Saves the provider.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        public async Task<int> SaveProvider(List<EntityProvider> entities)
		{
			await entities.ParallelForEachAsync(async entity =>
			{
				await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(entity),
					failureMessage: "DataContext failed to SAVE EntityProvider");
			});

			return await Task.Run(() => 1);
		}

        #endregion

        #region Payments

        /// <summary>
        /// Gets the provider payment.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<EntityProviderPayment> GetProviderPayment(string id)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityProviderPayment>().Where(o => o.Id == id).FirstOrDefaultAsync(),
			   failureMessage: "DataContext failed to load EntityProviderPayment");
		}

        /// <summary>
        /// Gets the provider payments by provider.
        /// </summary>
        /// <param name="providerId">The provider identifier.</param>
        /// <returns></returns>
        public async Task<List<EntityProviderPayment>> GetProviderPaymentsByProvider(string providerId)
		{
            return await Util.TryAsync(() => GetConnection().Table<EntityProviderPayment>().Where(o => o.ProviderId == providerId).ToListAsync(),
               failureMessage: "DataContext failed to load EntityProviderPayment");
        }

        /// <summary>
        /// Gets the provider payments by product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        public async Task<List<EntityProviderPayment>> GetProviderPaymentsByProduct(string productId)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityProviderPayment>().Where(o => o.ProductId == productId).ToListAsync(),
			   failureMessage: "DataContext failed to load TowEntityProviderPaymentns");
		}

        /// <summary>
        /// Gets the provider payments.
        /// </summary>
        /// <returns></returns>
        public async Task<List<EntityProviderPayment>> GetProviderPayments()
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityProviderPayment>().ToListAsync(),
               failureMessage: "DataContext failed to load EntityProviderPayment");
        }

        /// <summary>
        /// Saves the payment.
        /// </summary>
        /// <param name="providerPayment">The provider payment.</param>
        /// <returns></returns>
        public async Task<int> SavePayment(EntityProviderPayment providerPayment)
		{
            providerPayment.Id = UtilsService.GenerateId(providerPayment.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(providerPayment),
			   failureMessage: "DataContext failed to load Towns");
		}

        #endregion

        #region Provinces

        /// <summary>
        /// Gets the province.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<EntityProvince> GetProvince(string id)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityProvince>().Where(o => o.Id == id).FirstOrDefaultAsync(),
			   failureMessage: "DataContext failed to load EntityProvince");
		}

        /// <summary>
        /// Gets the provinces.
        /// </summary>
        /// <returns></returns>
        public async Task<List<EntityProvince>> GetProvinces()
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityProvince>().OrderBy(o => o.Name).ToListAsync(),
			   failureMessage: "DataContext failed to load EntityProvince");
		}

        /// <summary>
        /// Saves the province.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        public async Task<int> SaveProvince(List<EntityProvince> entities)
		{
			await entities.ParallelForEachAsync(async entity =>
			{
				await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(entity),
					failureMessage: "DataContext failed to SAVE EntityProvince");
			});

			return await Task.Run(() => 1);
		}

        /// <summary>
        /// Saves the province.
        /// </summary>
        /// <param name="province">The province.</param>
        /// <returns></returns>
        public async Task<int> SaveProvince(EntityProvince province)
        {
            province.Id = UtilsService.GenerateId(province.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(province),
             failureMessage: "DataContext failed to SAVE EntityProvince");
        }

        #endregion

        #region Towns

        /// <summary>
        /// Gets the town.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<EntityTown> GetTown(string id)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityTown>().Where(o => o.Id == id).FirstOrDefaultAsync(),
			   failureMessage: "DataContext {GetTown} failed to load EntityTown");
		}

        /// <summary>
        /// Gets the towns.
        /// </summary>
        /// <returns></returns>
        public async Task<List<EntityTown>> GetTowns()
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityTown>().OrderBy(o => o.Name).ToListAsync(),
			   failureMessage: "DataContext {GetTowns} failed to load EntityTown", onFailureReturn: new List<EntityTown>());
		}

        /// <summary>
        /// Gets the towns by province.
        /// </summary>
        /// <param name="provinceId">The province identifier.</param>
        /// <returns></returns>
        public async Task<List<EntityTown>> GetTownsByProvince(string provinceId)
		{ 
			return await Util.TryAsync(() => GetConnection().Table<EntityTown>().Where(o => o.ProvinceId == provinceId).ToListAsync(),
			   failureMessage: "DataContext {GetTownsByProvince} failed to load EntityTown");
		}

        /// <summary>
        /// Saves the town.
        /// </summary>
        /// <param name="town">The town.</param>
        /// <returns></returns>
        public async Task<int> SaveTown(EntityTown town)
		{
            town.Id = UtilsService.GenerateId(town.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(town),
			   failureMessage: "DataContext {SaveTown} failed to load EntityTown");
		}

        /// <summary>
        /// Saves the town.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        public async Task<int> SaveTown(List<EntityTown> entities)
		{
			await entities.ParallelForEachAsync(async entity =>
			{
                entity.Id = UtilsService.GenerateId(entity.Id);

                await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(entity),
                    failureMessage: "DataContext {SaveTown} failed to SAVE EntityTown");
            });

			return await Task.Run(() => 1);
		}

        #endregion

        #region Suburbs

        /// <summary>
        /// Gets the suburb.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<EntitySuburb> GetSuburb(string id)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntitySuburb>().Where(o => o.Id == id).FirstOrDefaultAsync(),
			   failureMessage: "DataContext failed to load EntitySuburb");
		}

        /// <summary>
        /// Gets the suburbs.
        /// </summary>
        /// <returns></returns>
        public async Task<List<EntitySuburb>> GetSuburbs()
		{
			return await Util.TryAsync(() => GetConnection().Table<EntitySuburb>().OrderBy(o => o.Name).ToListAsync(),
			   failureMessage: "DataContext failed to load EntitySuburb");
		}

        /// <summary>
        /// Gets the suburbs by town.
        /// </summary>
        /// <param name="townId">The town identifier.</param>
        /// <returns></returns>
        public async Task<List<EntitySuburb>> GetSuburbsByTown(string townId)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntitySuburb>().OrderBy(o => o.Name).ToListAsync(),
			   failureMessage: "DataContext failed to load EntitySuburb");
		}

        /// <summary>
        /// Saves the suburb.
        /// </summary>
        /// <param name="suburb">The suburb.</param>
        /// <returns></returns>
        public async Task<int> SaveSuburb(EntitySuburb suburb)
		{
            suburb.Id = UtilsService.GenerateId(suburb.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(suburb),
			   failureMessage: "DataContext failed to SAVE EntitySuburb");
		}

        /// <summary>
        /// Saves the suburb.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        public async Task<int> SaveSuburb(List<EntitySuburb> entities)
		{
			await entities.ParallelForEachAsync(async entity =>
			{
                entity.Id = UtilsService.GenerateId(entity.Id);

                await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(entity),
					failureMessage: "DataContext failed to SAVE EntitySuburb");
			});

			return await Task.Run(() => 1);
		}

        #endregion

        #region Street

        /// <summary>
        /// Gets the street.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<EntityStreet> GetStreet(string id)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityStreet>().Where(o => o.Id == id).FirstOrDefaultAsync(),
			   failureMessage: "DataContext failed to load Towns");
		}

        /// <summary>
        /// Gets the streets.
        /// </summary>
        /// <returns></returns>
        public async Task<List<EntityStreet>> GetStreets()
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityStreet>().OrderBy(o => o.Name).ToListAsync(),
			   failureMessage: "DataContext failed to load Towns", onFailureReturn: new List<EntityStreet>());
		}

        /// <summary>
        /// Gets the streets by suburb.
        /// </summary>
        /// <param name="suburbId">The suburb identifier.</param>
        /// <returns></returns>
        public async Task<List<EntityStreet>> GetStreetsBySuburb(string suburbId)
		{
            return await Util.TryAsync(() => GetConnection().Table<EntityStreet>().Where(o => o.SuburbId == suburbId).ToListAsync(),
               failureMessage: "DataContext failed to load Towns");
        }

        /// <summary>
        /// Saves the street.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<int> SaveStreet(EntityStreet entity)
		{
            entity.Id = UtilsService.GenerateId(entity.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(entity),
               failureMessage: "DataContext failed to load Towns");
        }

        /// <summary>
        /// Saves the street.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        public async Task<int> SaveStreet(List<EntityStreet> entities)
		{
			await entities.ParallelForEachAsync(async entity =>
			{
                entity.Id = UtilsService.GenerateId(entity.Id);

                await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(entity),
					failureMessage: "DataContext failed to SAVE EntityStreet");
			});

			return await Task.Run(() => 1);
		}

        #endregion

        #region Applications

        /// <summary>
        /// Saves the application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns></returns>
        public async Task<int> SaveApplication(EntityApplication application)
        {
            application.Id = UtilsService.GenerateId(application.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(application),
              failureMessage: "DataContext failed to SAVE EntityApplication");
        }

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<EntityApplication> GetApplication(string id)
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityApplication>().Where(o => o.Id == id).FirstOrDefaultAsync(),
               failureMessage: "DataContext failed to load EntityApplication");
        }

        /// <summary>
        /// Gets the sent applications.
        /// </summary>
        /// <returns></returns>
        public async Task<List<EntityApplication>> GetSentApplications()
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityApplication>().ToListAsync(),
               failureMessage: "DataContext failed to load EntityApplication");
        }

        #endregion
    }
}
