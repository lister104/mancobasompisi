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

        public MancobaLocalDataApi(IMvxSqliteConnectionFactory connectionFactory, IPlatformCapabilities platformCapabilities)
        {
            //#warning If anything changes in here, DO HARDWARE RESET ON iOS SIMULATOR        
            _connectionFactory = connectionFactory;
            _platformCapabilities = platformCapabilities;
           
            CreateTables();
        }

        private SQLiteAsyncConnection GetConnection()
        {
            if(_connection == null)
                _connection = _connectionFactory.GetAsyncConnection(SqlDbName);

            return _connection;
        }

        private SQLiteConnection GetSyncConnection()
        {
            if (_syncConnection == null)
                _syncConnection = _connectionFactory.GetConnection(SqlDbName);

            return _syncConnection;
        }

        #endregion

        #region IDisposable

        private bool _disposed = false;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

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

		~MancobaLocalDataApi()
		{
			Dispose(false);
		}

		#endregion

        #region Tables

        private static bool _hasCreatedTables;
        private static readonly object TableSync = new object();

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

        public async Task<EntitySystemUser> Login(string emailAddress, string password)
	    {			
			return await Util.TryAsync(() => GetConnection().Table<EntitySystemUser>().FirstOrDefaultAsync(),
				  failureMessage: "DataContext failed to find a EntitySystemUser");
		}

	    public async Task<int> Logout(string userId)
	    {		    
            return await Task.Run(()=>1);
		}

	    public async Task<int> Ping()
	    {
            return await Task.Run(() => 1);
        }

        public async Task<int> CheckIn(string userId)
		{
            return await Task.Run(() => 1);
        }

        public async Task<int> CheckOut(string userId)
		{
            return await Task.Run(() => 1);
        }

        #endregion

        #region Users

        public async Task<EntitySystemUser> GetSystemUserById(string id)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntitySystemUser>().Where(o => o.Id== id).FirstOrDefaultAsync(),
			   failureMessage: "DataContext failed to load EntitySystemUser");
		}

		public async Task<EntitySystemUser> GetAnySystemUser()
		{
			return await Util.TryAsync(() => GetConnection().Table<EntitySystemUser>().FirstOrDefaultAsync(),
			   failureMessage: "DataContext failed to load GetAnySystemUser");
		}

		public async Task<int> SaveSystemUser(EntitySystemUser town)
		{
			return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(town),
			   failureMessage: "DataContext failed to SAVE EntitySystemUser");
		}

		#endregion

		#region Installer

		public async Task<EntityInstaller> GetInstaller(string id)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityInstaller>().Where(o => o.Id == id).FirstOrDefaultAsync(),
			   failureMessage: "DataContext failed to load EntityInstaller");
		}

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

        public async Task<List<EntityInstaller>> GetInstallers()
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityInstaller>().ToListAsync(),
               failureMessage: "DataContext failed to load EntityInstaller");
        }

        public async Task<int> SaveInstaller(EntityInstaller installer)
		{
            installer.Id = UtilsService.GenerateId(installer.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(installer),
			  failureMessage: "DataContext failed to SAVE EntityInstaller");
		}

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

        public async Task<EntityProduct> GetProduct(string id)
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityProduct>().Where(o => o.Id == id).FirstOrDefaultAsync());
        }

        public async Task<List<EntityProduct>> GetProducts()
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityProduct>().ToListAsync());
        }

        public async Task<EntityProviderProduct> GetProviderProduct(string id)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityProviderProduct>().Where(o => o.Id == id).FirstOrDefaultAsync());
		}

		public async Task<List<EntityProviderProduct>> GetProviderProducts(string providerId)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityProviderProduct>().Where(o => o.ProviderId == providerId).ToListAsync());
		}

        public async Task<List<EntityProviderProduct>> GetProviderProducts()
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityProviderProduct>().ToListAsync());
        }

        public async Task<int> SaveProviderProduct(EntityProviderProduct providerProduct)
		{
            providerProduct.Id = UtilsService.GenerateId(providerProduct.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(providerProduct),
			  failureMessage: "DataContext failed to SAVE EntityProviderProduct");
		}

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

		public async Task<EntityProvider> GetProvider(string id)
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityProvider>().OrderBy(o => o.Id == id).FirstOrDefaultAsync(),
               failureMessage: "DataContext failed to load EntityProvider by GetAccount");
        }

        public async Task<List<EntityProvider>> GetProviders()
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityProvider>().ToListAsync(),
               failureMessage: "DataContext failed to load EntityProvider");
        }

        public async Task<List<EntityProvider>> FindProviders(string searchText)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityProvider>().Where(o => o.Name == searchText).ToListAsync(),
			   failureMessage: "DataContext failed to load EntityProvider");
		}

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

        public async Task<int> SaveProvider(EntityProvider provider)
		{
            provider.Id = UtilsService.GenerateId(provider.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(provider),
			   failureMessage: "DataContext failed to SAVE EntityProvider");
		}

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

		public async Task<EntityProviderPayment> GetProviderPayment(string id)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityProviderPayment>().Where(o => o.Id == id).FirstOrDefaultAsync(),
			   failureMessage: "DataContext failed to load EntityProviderPayment");
		}

		public async Task<List<EntityProviderPayment>> GetProviderPaymentsByProvider(string providerId)
		{
            return await Util.TryAsync(() => GetConnection().Table<EntityProviderPayment>().Where(o => o.ProviderId == providerId).ToListAsync(),
               failureMessage: "DataContext failed to load EntityProviderPayment");
        }

		public async Task<List<EntityProviderPayment>> GetProviderPaymentsByProduct(string productId)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityProviderPayment>().Where(o => o.ProductId == productId).ToListAsync(),
			   failureMessage: "DataContext failed to load TowEntityProviderPaymentns");
		}

        public async Task<List<EntityProviderPayment>> GetProviderPayments()
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityProviderPayment>().ToListAsync(),
               failureMessage: "DataContext failed to load EntityProviderPayment");
        }

        public async Task<int> SavePayment(EntityProviderPayment providerPayment)
		{
            providerPayment.Id = UtilsService.GenerateId(providerPayment.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(providerPayment),
			   failureMessage: "DataContext failed to load Towns");
		}

		#endregion

		#region Provinces

		public async Task<EntityProvince> GetProvince(string id)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityProvince>().Where(o => o.Id == id).FirstOrDefaultAsync(),
			   failureMessage: "DataContext failed to load EntityProvince");
		}

		public async Task<List<EntityProvince>> GetProvinces()
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityProvince>().OrderBy(o => o.Name).ToListAsync(),
			   failureMessage: "DataContext failed to load EntityProvince");
		}

		public async Task<int> SaveProvince(List<EntityProvince> entities)
		{
			await entities.ParallelForEachAsync(async entity =>
			{
				await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(entity),
					failureMessage: "DataContext failed to SAVE EntityProvince");
			});

			return await Task.Run(() => 1);
		}

        public async Task<int> SaveProvince(EntityProvince province)
        {
            province.Id = UtilsService.GenerateId(province.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(province),
             failureMessage: "DataContext failed to SAVE EntityProvince");
        }

        #endregion

        #region Towns

        public async Task<EntityTown> GetTown(string id)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityTown>().Where(o => o.Id == id).FirstOrDefaultAsync(),
			   failureMessage: "DataContext {GetTown} failed to load EntityTown");
		}

		public async Task<List<EntityTown>> GetTowns()
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityTown>().OrderBy(o => o.Name).ToListAsync(),
			   failureMessage: "DataContext {GetTowns} failed to load EntityTown", onFailureReturn: new List<EntityTown>());
		}
		
		public async Task<List<EntityTown>> GetTownsByProvince(string provinceId)
		{ 
			return await Util.TryAsync(() => GetConnection().Table<EntityTown>().Where(o => o.ProvinceId == provinceId).ToListAsync(),
			   failureMessage: "DataContext {GetTownsByProvince} failed to load EntityTown");
		}

		public async Task<int> SaveTown(EntityTown town)
		{
            town.Id = UtilsService.GenerateId(town.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(town),
			   failureMessage: "DataContext {SaveTown} failed to load EntityTown");
		}

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

		public async Task<EntitySuburb> GetSuburb(string id)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntitySuburb>().Where(o => o.Id == id).FirstOrDefaultAsync(),
			   failureMessage: "DataContext failed to load EntitySuburb");
		}

		public async Task<List<EntitySuburb>> GetSuburbs()
		{
			return await Util.TryAsync(() => GetConnection().Table<EntitySuburb>().OrderBy(o => o.Name).ToListAsync(),
			   failureMessage: "DataContext failed to load EntitySuburb");
		}

		public async Task<List<EntitySuburb>> GetSuburbsByTown(string townId)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntitySuburb>().OrderBy(o => o.Name).ToListAsync(),
			   failureMessage: "DataContext failed to load EntitySuburb");
		}

		public async Task<int> SaveSuburb(EntitySuburb suburb)
		{
            suburb.Id = UtilsService.GenerateId(suburb.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(suburb),
			   failureMessage: "DataContext failed to SAVE EntitySuburb");
		}

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

		public async Task<EntityStreet> GetStreet(string id)
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityStreet>().Where(o => o.Id == id).FirstOrDefaultAsync(),
			   failureMessage: "DataContext failed to load Towns");
		}

		public async Task<List<EntityStreet>> GetStreets()
		{
			return await Util.TryAsync(() => GetConnection().Table<EntityStreet>().OrderBy(o => o.Name).ToListAsync(),
			   failureMessage: "DataContext failed to load Towns", onFailureReturn: new List<EntityStreet>());
		}

		public async Task<List<EntityStreet>> GetStreetsBySuburb(string suburbId)
		{
            return await Util.TryAsync(() => GetConnection().Table<EntityStreet>().Where(o => o.SuburbId == suburbId).ToListAsync(),
               failureMessage: "DataContext failed to load Towns");
        }

		public async Task<int> SaveStreet(EntityStreet entity)
		{
            entity.Id = UtilsService.GenerateId(entity.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(entity),
               failureMessage: "DataContext failed to load Towns");
        }

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

        public async Task<int> SaveApplication(EntityApplication application)
        {
            application.Id = UtilsService.GenerateId(application.Id);

            return await Util.TryAsync(() => GetConnection().InsertOrReplaceAsync(application),
              failureMessage: "DataContext failed to SAVE EntityApplication");
        }

        public async Task<EntityApplication> GetApplication(string id)
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityApplication>().Where(o => o.Id == id).FirstOrDefaultAsync(),
               failureMessage: "DataContext failed to load EntityApplication");
        }

        public async Task<List<EntityApplication>> GetSentApplications()
        {
            return await Util.TryAsync(() => GetConnection().Table<EntityApplication>().ToListAsync(),
               failureMessage: "DataContext failed to load EntityApplication");
        }

        #endregion
    }
}
