using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mancoba.Sompisi.Data.LocalDb.Entities;
using Mancoba.Sompisi.Utils.Helpers;
using Mancoba.Sompisi.Utils.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Plugins.Sqlite;

namespace Mancoba.Sompisi.Data.LocalDb
{
	internal class TestData
	{
		private const string FormatDate = "yyyy-MM-dd";
		private const string FormatDateTime = "yyyy-MM-dd HH:mm:ss";
		internal static IMvxSqliteConnectionFactory ConnectionFactory;
		internal static IPlatformCapabilities PlatformCapabilities;

        /// <summary>
        /// Initializes the <see cref="TestData"/> class.
        /// </summary>
        static TestData()
		{
			if(ConnectionFactory == null)
				ConnectionFactory = Mvx.Resolve<IMvxSqliteConnectionFactory>();

			if (PlatformCapabilities == null)
				PlatformCapabilities = Mvx.Resolve<IPlatformCapabilities>();			
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="TestData"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="platformCapabilities">The platform capabilities.</param>
        public TestData(IMvxSqliteConnectionFactory connectionFactory, IPlatformCapabilities platformCapabilities)
		{
			ConnectionFactory = connectionFactory;
			PlatformCapabilities = platformCapabilities;
		}

		#region EntitySystemUser

		private static List<EntitySystemUser> _users = new List<EntitySystemUser>();

        /// <summary>
        /// Gets the system user.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        internal static async Task<EntitySystemUser> GetSystemUser(string emailAddress, string password)
		{
			if (_users == null)
				_users = new List<EntitySystemUser>();

			EntitySystemUser user = _users.FirstOrDefault();

			if (user == null)
			{
				var streets = await GetStreets();

				user = new EntitySystemUser();
				user.Id = UtilsService.GenerateId(user.Id);
				user.FirstName = "System";
				user.LastName = "Administrator";
				user.Username = emailAddress;
				user.Password = password;

				user.EmailAddress = "info@mancoba.co.za";
				user.PhoneNumber = "021 531 7856";
				user.MobileNumber = "082 421 1330";

				SetAddress(streets, UtilsService.RandomNumber(11, 99).ToString(), ref user);

				using (var db = new MancobaLocalDataApi(ConnectionFactory, PlatformCapabilities))
				{
					var response = await db.SaveSystemUser(user);
				}
			}

			return user;
		}

        /// <summary>
        /// Sets the address.
        /// </summary>
        /// <param name="streets">The streets.</param>
        /// <param name="name">The name.</param>
        /// <param name="entity">The entity.</param>
        private static void SetAddress(List<EntityStreet> streets, string name, ref EntitySystemUser entity)
		{
			var str = streets[UtilsService.RandomNumber(0, streets.Count - 1)];
			var sub = _suburbList.FirstOrDefault(c => c.Id == str.SuburbId);
			var town = _townList.FirstOrDefault(c => c.Id == sub.TownId);
			var prov = _provinceList.FirstOrDefault(c => c.Id == town.ProvinceId);

			entity.StreetId = str.Id;
			entity.House = name;
			entity.Street = str.Name;
			entity.Suburb = sub.Name;
			entity.Town = town.Name;
			entity.Province = prov.Name;
			entity.Country = prov.Country;
		}

		#endregion

		#region EntityProvider

		private static List<EntityProvider> _providers = new List<EntityProvider>();

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <returns></returns>
        internal static async Task<List<EntityProvider>> GetProviders()
        {

            if (_providers == null)
                _providers = new List<EntityProvider>();

            if (_providers.Count == 0)
            {
                var streets = await GetStreets();

                var dto1 = new EntityProvider();
                dto1.Id = UtilsService.GenerateId(dto1.Id);
                dto1.Name = "MultiChoice South Africa";
                dto1.ContactPerson = "Sales Team";
                dto1.EmailAddress = "sales@multichoice.co.za";
                dto1.PhoneNumber = "+27 21 508 1234";
                dto1.MobileNumber = null;
                dto1.WebAddress = "http://www.multichoice.co.za";
                dto1.IsFavourite = false;

                SetAddress(streets, "15, DStv House", ref dto1);
                _providers.Add(dto1);

                var dto2 = new EntityProvider();
                dto2.Id = UtilsService.GenerateId(dto2.Id);
                dto2.Name = "MultiChoice Africa";
                dto2.ContactPerson = "Sales Manager";
                dto2.EmailAddress = "manager@multichoiceafrica.com";
                dto2.PhoneNumber = "+27 11 218 2342";
                dto2.MobileNumber = "+27 82 131 3893";
                dto2.WebAddress = "http://www.multichoice.co.za";
                dto2.IsFavourite = false;

                SetAddress(streets, "64, Africa House", ref dto2);
                _providers.Add(dto2);
            }

            using (var db = new MancobaLocalDataApi(ConnectionFactory, PlatformCapabilities))
            {
                var response = await db.SaveProvider(_providers);
            }

            return _providers.OrderBy(c => c.Name).ToList();
        }

        /// <summary>
        /// Sets the address.
        /// </summary>
        /// <param name="streets">The streets.</param>
        /// <param name="name">The name.</param>
        /// <param name="entity">The entity.</param>
        private static void SetAddress(List<EntityStreet> streets, string name, ref EntityProvider entity)
	    {
	        var str = streets[UtilsService.RandomNumber(0, streets.Count - 1)];
	        var sub = _suburbList.FirstOrDefault(c => c.Id == str.SuburbId);
	        var town = _townList.FirstOrDefault(c => c.Id == sub.TownId);
	        var prov = _provinceList.FirstOrDefault(c => c.Id == town.ProvinceId);

	        entity.StreetId = str.Id;
	        entity.House = name;
	        entity.Street = str.Name;
	        entity.Suburb = sub.Name;
	        entity.Town = town.Name;
	        entity.Province = prov.Name;
	        entity.Country = prov.Country;
	    }

	    #endregion

		#region EntityInstaller

		private static List<EntityInstaller> _installers = new List<EntityInstaller>();

        /// <summary>
        /// Gets the installers.
        /// </summary>
        /// <returns></returns>
        internal static async Task<List<EntityInstaller>> GetInstallers()
		{
			if (_installers == null)
				_installers = new List<EntityInstaller>();

			if (_installers.Count == 0)
			{
				var streets = await GetStreets();

				foreach (var town in _townList)
				{
					var subs = _suburbList.Where(c => c.TownId == town.Id).Select(c => c.Id).ToList();
					var sts = _streetList.Where(c => subs.Contains(c.SuburbId)).ToList();

					var dto1 = new EntityInstaller();
					dto1.Id = UtilsService.GenerateId(dto1.Id);
					dto1.Name = $"Test Installer {UtilsService.RandomNumber(111, 999)}";
					dto1.ContactPerson = "Mr Owner";
					dto1.EmailAddress = $"installer@{dto1.Name.ToLower()}.co.za";
					dto1.PhoneNumber =
						$"+27 {UtilsService.RandomNumber(11, 99)} {UtilsService.RandomNumber(111, 999)} {UtilsService.RandomNumber(1111, 9999)}";
					dto1.MobileNumber =
						$"+27 {UtilsService.RandomNumber(60, 89)} {UtilsService.RandomNumber(111, 999)} {UtilsService.RandomNumber(1111, 9999)}";
					dto1.IsFavourite = false;
                    SetAddress(streets, ref dto1);

                    _installers.Add(dto1);

					var dto2 = new EntityInstaller();
					dto2.Id = UtilsService.GenerateId(dto1.Id);
					dto2.Name = $"Test Installer {UtilsService.RandomNumber(111, 999)}";
					dto2.ContactPerson = "Mr Owner";
					dto2.EmailAddress = $"installer@{dto1.Name.ToLower()}.co.za";
					dto2.PhoneNumber =
						$"+27 {UtilsService.RandomNumber(11, 99)} {UtilsService.RandomNumber(111, 999)} {UtilsService.RandomNumber(1111, 9999)}";
					dto2.MobileNumber =
						$"+27 {UtilsService.RandomNumber(60, 89)} {UtilsService.RandomNumber(111, 999)} {UtilsService.RandomNumber(1111, 9999)}";
                    dto2.IsFavourite = false;

				    SetAddress(streets, ref dto2);                   
					_installers.Add(dto2);
				}

				using (var db = new MancobaLocalDataApi(ConnectionFactory, PlatformCapabilities))
				{
					var response = await db.SaveInstaller(_installers);
				}
			}

			return _installers.OrderBy(c => c.Name).ToList();
		}

        /// <summary>
        /// Sets the address.
        /// </summary>
        /// <param name="streets">The streets.</param>
        /// <param name="entity">The entity.</param>
        private static void SetAddress(List<EntityStreet> streets, ref EntityInstaller entity)
        {
            var str = streets[UtilsService.RandomNumber(0, streets.Count - 1)];
            var sub = _suburbList.FirstOrDefault(c => c.Id == str.SuburbId);
            var town = _townList.FirstOrDefault(c => c.Id == sub.TownId);
            var prov = _provinceList.FirstOrDefault(c => c.Id == town.ProvinceId);

            entity.StreetId = str.Id;
            entity.House = $"{UtilsService.RandomNumber(11, 99)}";
            entity.Street = str.Name;
            entity.Suburb = sub.Name;
            entity.Town = town.Name;
            entity.Province = prov.Name;
            entity.Country = prov.Country;
        }

        #endregion

        #region EntityProduct

        private static List<EntityProduct> _products = new List<EntityProduct>();

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <returns></returns>
        internal static async Task<List<EntityProduct>> GetProducts()
        {

            if (_products == null)
                _products = new List<EntityProduct>();

            if (_products.Count == 0)
            {
                _products.Add(new EntityProduct()
                {
                    Id = UtilsService.GenerateId(),
                    Name = "PVR Decoder",
                    Description = "DStv PVR Decoder",
                    Price = 698
                });

                _products.Add(new EntityProduct()
                {
                    Id = UtilsService.GenerateId(),
                    Name = "Single-View Decoder",
                    Description = "DStv Single-View Decoder",
                    Price = 350
                });

                _products.Add(new EntityProduct()
                {
                    Id = UtilsService.GenerateId(),
                    Name = "Dual -View Decoder",
                    Description = "DStv Dual-View Decoder",
                    Price = 450
                });

                _products.Add(new EntityProduct()
                {
                    Id = UtilsService.GenerateId(),
                    Name = "Explora Decoder",
                    Description = "DStv Explora Decoder",
                    Price = 1300
                });

                _products.Add(new EntityProduct()
                {
                    Id = UtilsService.GenerateId(),
                    Name = "2-Tuner PVR Decoder",
                    Description = "DStv 2-Tuner Decoder",
                    Price = 799
                });

                _products.Add(new EntityProduct()
                {
                    Id = UtilsService.GenerateId(),
                    Name = "SD PVR Decoder",
                    Description = "DStv SD PVR Decoder",
                    Price = 900
                });

                _products.Add(new EntityProduct()
                {
                    Id = UtilsService.GenerateId(),
                    Name = "UEC 4-tuner PVR Decoder",
                    Description = "DStv UEC 4-tuner PVR Decoder",
                    Price = 900
                });

            }

            using (var db = new MancobaLocalDataApi(ConnectionFactory, PlatformCapabilities))
            {
                var response = await db.SaveProduct(_products);
            }

            return _products.OrderBy(c => c.Name).ToList();
        }

        #endregion

        #region Addresses

        private static List<EntityProvince> _provinceList = new List<EntityProvince>();
		private static List<EntityTown> _townList = new List<EntityTown>();
		private static List<EntitySuburb> _suburbList = new List<EntitySuburb>();
		private static List<EntityStreet> _streetList = new List<EntityStreet>();

        /// <summary>
        /// Gets the provinces.
        /// </summary>
        /// <returns></returns>
        public static async Task<List<EntityProvince>> GetProvinces()
		{
			if (_provinceList == null || _provinceList.Count == 0)
			{
				const string country = "South Africa";
				_provinceList = new List<EntityProvince>()
				{
					new EntityProvince()
					{
						Country = country,
						Id = UtilsService.GenerateId(),
						Name = "Eastern Cape"
					},
					new EntityProvince()
					{
						Country = country,
						Id = UtilsService.GenerateId(),
						Name = "Free State"
					},
					new EntityProvince()
					{
						Country = country,
						Id = UtilsService.GenerateId(),
						Name = "Gauteng"
					},
					new EntityProvince()
					{
						Country = country,
						Id = UtilsService.GenerateId(),
						Name = "KwaZulu-Natal"
					},
					new EntityProvince()
					{
						Country = country,
						Id = UtilsService.GenerateId(),
						Name = "Limpopo"
					},
					new EntityProvince()
					{
						Country = country,
						Id = UtilsService.GenerateId(),
						Name = "Mpumalanga"
					},
					new EntityProvince()
					{
						Country = country,
						Id = UtilsService.GenerateId(),
						Name = "Northern Cape"
					},
					new EntityProvince()
					{
						Country = country,
						Id = UtilsService.GenerateId(),
						Name = "North West"
					},
					new EntityProvince()
					{
						Country = country,
						Id = UtilsService.GenerateId(),
						Name = "Western Cape"
					}
				};

				using (var db = new MancobaLocalDataApi(ConnectionFactory, PlatformCapabilities))
				{
					var response = await db.SaveProvince(_provinceList);
				}

			}

			return _provinceList.OrderBy(c => c.Name).ToList();
		}

        /// <summary>
        /// Gets the name of the province by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static async Task<EntityProvince> GetProvinceByName(string name)
		{
			return await Task.Run(async () =>
			{
				if (_provinceList == null || _provinceList.Count == 0)
					await GetProvinces();

				if (_provinceList == null)
					return null;

				return _provinceList.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase));
			});
		}

        /// <summary>
        /// Gets the towns.
        /// </summary>
        /// <returns></returns>
        public static async Task<List<EntityTown>> GetTowns()
		{
			if (_townList == null || _townList.Count == 0)
			{
				_townList = new List<EntityTown>();

				for (int i = 0; i < _provinceList.Count; i++)
				{
					var province = _provinceList[i];

					if (i == 0) //  Name = "Eastern Cape"
					{
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Port Elizabeth",
							ProvinceId = province.Id,
						});
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "East London",
							ProvinceId = province.Id,
						});
					}
					if (i == 1) //  Name = "Free State"
					{
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Bloemfontein",
							ProvinceId = province.Id,
						});
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Welkom",
							ProvinceId = province.Id,
						});
					}
					if (i == 2) //  Name = "Gauteng"
					{
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Pretoria",
							ProvinceId = province.Id,
						});
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Johannesburg",
							ProvinceId = province.Id,
						});
					}

					if (i == 3) //  Name = "KwaZulu-Natal"
					{
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Durban",
							ProvinceId = province.Id,
						});
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Pietermaritzburg",
							ProvinceId = province.Id,
						});
					}

					if (i == 4) //  Name = "Limpopo"
					{
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Mokopane",
							ProvinceId = province.Id,
						});
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Polokwane",
							ProvinceId = province.Id,
						});
					}

					if (i == 5) //  Name = "Mpumalanga"
					{
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Ermelo",
							ProvinceId = province.Id,
						});
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Nelspruit",
							ProvinceId = province.Id,
						});
					}

					if (i == 6) //  Name = "Northern Cape"
					{
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Kimberley",
							ProvinceId = province.Id,
						});
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Upington",
							ProvinceId = province.Id,
						});
					}

					if (i == 7) //  Name = "North West"
					{
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Rustenburg",
							ProvinceId = province.Id,
						});
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Potchefstroom",
							ProvinceId = province.Id,
						});
					}

					if (i == 8) //  Name = "Western Cape"
					{
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Cape Town",
							ProvinceId = province.Id,
						});
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Mossel Bay",
							ProvinceId = province.Id,
						});
						_townList.Add(new EntityTown()
						{
							Id = UtilsService.GenerateId(),
							Name = "Worcester",
							ProvinceId = province.Id,
						});
					}
				}

				using (var db = new MancobaLocalDataApi(ConnectionFactory, PlatformCapabilities))
				{
					var response = await db.SaveTown(_townList);
				}
			}

			return _townList.OrderBy(c => c.Name).ToList();
		}

        /// <summary>
        /// Gets the name of the town by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static async Task<EntityTown> GetTownByName(string name)
		{
			return await Task.Run(async () =>
			{
				if (_townList == null || _townList.Count == 0)
					await GetTowns();

				if (_townList == null)
					return null;

				return _townList.FirstOrDefault(p => p.Name.AreEqualIgnoreCase(name));
			});
		}

        /// <summary>
        /// Gets the suburbs.
        /// </summary>
        /// <returns></returns>
        public static async Task<List<EntitySuburb>> GetSuburbs()
		{
			if (_suburbList == null || _suburbList.Count == 0)
			{
				_suburbList = new List<EntitySuburb>();

				foreach (var town in _townList)
				{					
					_suburbList.Add(new EntitySuburb()
					{
						Name = "Pinelands",
						TownId = town.Id,
					});
					
					_suburbList.Add(new EntitySuburb()
					{
						Name = "Claremont",
						TownId = town.Id,
					});
					
					_suburbList.Add(new EntitySuburb()
					{
						Name = "Centurion",
						TownId = town.Id,
					});

					_suburbList.Add(new EntitySuburb()
					{
						Name = "Newtown",
						TownId = town.Id,
					});

					_suburbList.Add(new EntitySuburb()
					{
						Name = "Sandton",
						TownId = town.Id,
					});
				}

				using (var db = new MancobaLocalDataApi(ConnectionFactory, PlatformCapabilities))
				{
					var response = await db.SaveTown(_townList);
				}
			}

			return _suburbList.OrderBy(c => c.Name).ToList();
		}

        /// <summary>
        /// Gets the streets.
        /// </summary>
        /// <returns></returns>
        public static async Task<List<EntityStreet>> GetStreets()
		{

			if (_streetList == null || _streetList.Count == 0)
			{
				_streetList = new List<EntityStreet>();

				foreach (var sub in _suburbList)
				{
					string txt = UtilsService.RandomNumber(1, 100) > 50 ? "Road" : "Street";
					_streetList.Add(new EntityStreet()
					{
						Name = $"Main {txt}",
						SuburbId = sub.Id,
					});

					txt = UtilsService.RandomNumber(1, 100) > 50 ? "Road" : "Street";
					_streetList.Add(new EntityStreet()
					{
						Name = $"Voortrekker {txt}",
						SuburbId = sub.Id,
					});

					int road = UtilsService.RandomNumber(1, 9);
					_streetList.Add(new EntityStreet()
					{
						Name = $"M {road}",
						SuburbId = sub.Id,
					});
				}


				using (var db = new MancobaLocalDataApi(ConnectionFactory, PlatformCapabilities))
				{
					var response = await db.SaveTown(_townList);
				}
			}

			return _streetList.OrderBy(c => c.Name).ToList();

		}

        #endregion

        #region Applications

        private static List<EntityApplication> _applications = new List<EntityApplication>();

        /// <summary>
        /// Gets the sent applications.
        /// </summary>
        /// <returns></returns>
        internal static async Task<List<EntityApplication>> GetSentApplications()
        {
            if (_applications == null)
                _applications = new List<EntityApplication>();

            if (_applications.Count == 0)
            {
                var streets = await GetStreets();

                foreach (var town in _townList)
                {
                    var subs = _suburbList.Where(c => c.TownId == town.Id).Select(c => c.Id).ToList();
                    var sts = _streetList.Where(c => subs.Contains(c.SuburbId)).ToList();

                    var dto1 = new EntityApplication();
                    dto1.Id = UtilsService.GenerateId(dto1.Id);
                    dto1.FirstName = $"Brown";
                    dto1.LastName = "Njemza";
                    dto1.EmailAddress = "njemza@mancoba.co.za";
                    dto1.PhoneNumber = $"+27 {UtilsService.RandomNumber(11, 99)} {UtilsService.RandomNumber(111, 999)} {UtilsService.RandomNumber(1111, 9999)}";
                    dto1.MobileNumber = $"+27 {UtilsService.RandomNumber(60, 89)} {UtilsService.RandomNumber(111, 999)} {UtilsService.RandomNumber(1111, 9999)}";
                    SetAddress(streets, ref dto1);
                    _applications.Add(dto1);

                    var dto2 = new EntityApplication();
                    dto2.Id = UtilsService.GenerateId(dto1.Id);
                    dto2.FirstName = $"Bubu Mlibo";
                    dto2.LastName = "Mbiza";
                    dto2.EmailAddress = "bubu_mlibo@mancoba.co.za";
                    dto2.PhoneNumber = $"+27 {UtilsService.RandomNumber(11, 99)} {UtilsService.RandomNumber(111, 999)} {UtilsService.RandomNumber(1111, 9999)}";
                    dto2.MobileNumber = $"+27 {UtilsService.RandomNumber(60, 89)} {UtilsService.RandomNumber(111, 999)} {UtilsService.RandomNumber(1111, 9999)}";

                    SetAddress(streets, ref dto2);
                    _applications.Add(dto2);
                }

                using (var db = new MancobaLocalDataApi(ConnectionFactory, PlatformCapabilities))
                {
                    var response = await db.SaveApplication(_applications.FirstOrDefault());
                }
            }

            return _applications.OrderBy(c => c.FirstName).ToList();
        }

        /// <summary>
        /// Sets the address.
        /// </summary>
        /// <param name="streets">The streets.</param>
        /// <param name="entity">The entity.</param>
        private static void SetAddress(List<EntityStreet> streets, ref EntityApplication entity)
        {
            var str = streets[UtilsService.RandomNumber(0, streets.Count - 1)];
            var sub = _suburbList.FirstOrDefault(c => c.Id == str.SuburbId);
            var town = _townList.FirstOrDefault(c => c.Id == sub.TownId);
            var prov = _provinceList.FirstOrDefault(c => c.Id == town.ProvinceId);
            
            entity.Street = str.Name;
            entity.Suburb = sub.Name;
            entity.Town = town.Name;
            entity.Province = prov.Name;
            entity.Country = prov.Country;
            entity.PostalCode = $"{UtilsService.RandomNumber(1111, 9999)}";
        }

        #endregion
    }
}
