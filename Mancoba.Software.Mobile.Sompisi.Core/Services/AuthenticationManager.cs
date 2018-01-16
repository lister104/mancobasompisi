using System;
using System.Threading.Tasks;
using Bluescore.DStv.ApiClient;
using Bluescore.DStv.Core.Language;
using Bluescore.DStv.Core.Services.Contracts;
using Bluescore.DStv.Core.Settings;
using Bluescore.DStv.Utils.Helpers.UserInteraction;
using MvvmCross.Platform;
using Plugin.Connectivity;

namespace Bluescore.DStv.Core.Services
{
	public class AuthenticationManager : IAuthenticationManager
	{
		private readonly IBluescoreTvWebApi _apiService;

		public AuthenticationManager(IBluescoreTvWebApi apiService)
		{
			_apiService = apiService;
		}

		public async Task<bool> Ping()
		{
			bool success = false;
			if (!CrossConnectivity.Current.IsConnected)
			{
				Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.NoConnectivity);
				return false;
			}
			try
			{
				success = await _apiService.Ping();			
			}
			catch (Exception ex)
			{
				Mvx.Resolve<IUserInteraction>().ToastErrorAlert(ex.Message);
			}

			return success;
		}

		public async Task<bool> Login(string username, string password, string accessCode)
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.NoConnectivity);
				return false;
			}
			try
			{				
				var loginResult = await _apiService.Login(username, password, accessCode);
					
				var userSettings = Mvx.Resolve<IUserSettings>();
				userSettings.Username = username;
				userSettings.Password = password;

				return loginResult.IsSuccessful;
			}
			catch (Exception ex)
			{
				Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.LoginErrorTitle);				
			}

			return false;
		}

		public async Task<bool> TryAutoLogin()
		{		
			try
			{
				var userSettings = Mvx.Resolve<IUserSettings>();
				await _apiService.Login(userSettings.Username, userSettings.Password, userSettings.AccessCode); 

				return true;
			}
			catch
			{
				Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.LoginErrorTitle);
				return false;				
			}


		}
	}
}

