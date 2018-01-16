using System;
using System.Threading.Tasks;
using Bluescore.DStv.ApiClient;
using Bluescore.DStv.Core.Settings;
using MvvmCross.Platform;

namespace Bluescore.DStv.Core.Services
{
	public class ApiDataService2 : BluescoreTvWebApi
	{
		private static ApiDataService2 _instance;

		public static System.Net.Http.HttpMessageHandler MessageHandler;
		private static readonly string _serviceUsername = "rootuser@mixtel.com";
		private static readonly string _servicePassword = "dynamix_is_awesome";

		public ApiDataService2()
			//: base(_serviceUsername, _servicePassword)
		{
			//this.ApiHost = apiDataServiceUrl;
		}

		//public string Server { set { ApiHost = value; } }

		public static ApiDataService2 Instance()
		{
			IUserSettings userSettings = Mvx.Resolve<IUserSettings>();

			if (_instance == null)
			{
				//TechnitionToolMobileApiClient apiClient = new TechnitionToolMobileApiClient("http://10.34.202.218/TechnitionTool.Mobile.Api/serversList.JSON");
			
				_instance = new ApiDataService2();

				Task.Run (async () => {
					try
					{					
						var serverResult = await _instance.Ping();
					}
					catch (Exception ex)
					{						
					}

				});
			}

			return _instance;
		}
	}
}

