using System.Linq;
using System.Threading.Tasks;
using Mancoba.Sompisi.Core.Services.Contracts;
using Mancoba.Sompisi.Core.Validators;
using Mancoba.Sompisi.Core.ViewModels.Base;
using Mancoba.Sompisi.Utils.Helpers.UserInteraction;
using Mancoba.Sompisi.Utils.Interfaces;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.ViewModels
{
	public class LoginViewModel : BaseValidationViewModel
	{
		#region Private Variables

	
		private readonly IUserSettings _userSettings;
		private readonly IMobileDataService _auth;

		private readonly LoginValidator _validator;	
		private bool _isWidgetHidden;
		private bool _isWidgetVisible;		
			
		private string _emailAddres;
		private string _password;		

		private IMvxAsyncCommand _pingServerCommand;
        private IMvxAsyncCommand _termsCommand;

        #endregion

        #region Constructors
        public LoginViewModel(IMvxMessenger messenger, IUserSettings userSettings, IMobileDataService auth) : base(messenger)
		{
			_auth = auth;
			_userSettings = userSettings;
			_validator = new LoginValidator();
			
			LoginCommand = new MvxAsyncCommand(Login);

            EmailAddres = "info@mancoba.co.za";
            Task.Run(() => _auth.GetStreets());
        }
		#endregion

		#region Public Properties

		public string AppName => LanguageResolver.AppName;
		public string EmailAddressPlaceholder => LanguageResolver.LoginEmailAddress.ToUpperInvariant();
		public string PasswordPlaceholder => LanguageResolver.LoginPassword.ToUpperInvariant();	
		public string LoginButtonTitle => LanguageResolver.LoginButton.ToUpperInvariant();
		public string LoginTerms => LanguageResolver.TermsAndConditionsTitle.ToUpperInvariant();
		public string LoginForgotPassword => LanguageResolver.ForgotPasswordTitle;

		public bool IsWidgetHidden
		{
			get
			{
				return _isWidgetHidden;
			}
			set
			{
				_isWidgetHidden = value;
				RaisePropertyChanged(() => IsWidgetHidden);
			}
		}

		public bool IsWidgetVisible
		{
			get
			{
				return _isWidgetVisible;
			}
			set
			{
				_isWidgetVisible = value;
				RaisePropertyChanged(() => IsWidgetVisible);
			}
		}

		public string EmailAddres
		{
			get { return _emailAddres; }
			set
			{
                _emailAddres = value.Trim ();
				RaisePropertyChanged(() => EmailAddres);

			}
		}

		public string Password
		{
			get { return _password; }
			set
			{
				_password = value;
				RaisePropertyChanged(() => Password);
			}
		}
		
        public IMvxAsyncCommand TermsCommand
        {
            get
            {
                _termsCommand = _termsCommand ?? new MvxAsyncCommand(DoTermsCommand);
                return _termsCommand;
            }
        }

        public IMvxAsyncCommand LoginCommand { get; }

		public IMvxAsyncCommand PingServerCommand
		{
			get
			{
				_pingServerCommand = _pingServerCommand ?? new MvxAsyncCommand(DoPingServer);
				return _pingServerCommand;
			}
		}

		#endregion

		#region Private Methods

		#endregion

		#region Public Methods

		public void Init()
		{           
            DoPingServer();

			SetWidgetsVisiblity(true);

			if(!string.IsNullOrEmpty(_userSettings.Username) && !string.IsNullOrEmpty(_userSettings.Password) && !string.IsNullOrEmpty(_userSettings.AccessCode))
			{
				DoAutoLogin();
			}
		}

		public async Task DoAutoLogin()
		{
			IsLoading = true;
			SetWidgetsVisiblity(false);
			var loginResult = await _auth.TryAutoLogin();

			if(loginResult)
			{
				ShowViewModel<NavigationViewModel>();
			}
			else
			{
				Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.LoginErrorTitle, duration:3000);
			}

			SetWidgetsVisiblity(true);
			IsLoading = false;
		}

        public async Task DoTermsCommand()
        {            
            await Mvx.Resolve<IUserInteraction>().AlertAsync("Coming Soon!", "Terms and Conditions", LanguageResolver.Ok);
        }

        public async Task DoPingServer()
		{
			await _auth.Ping();
		}

		

		public async Task ShowTerms()
		{
			ShowViewModel<TermsViewModel>();
		}


		public async Task Login()
		{
			if (ValidationErrors != null)
				ValidationErrors.Clear();

			ValidateModel(_validator, this);

			if (IsModelValid)
			{
				IsLoading = true;

				//string username = "chipo";
				//string password = "chipo001";

				//bool success = await _auth.Login(username, password, "");
				bool success = await _auth.Login(EmailAddres, Password);

				if (success)
				{
					ShowViewModel<NavigationViewModel>();
				}
				else
				{
					Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.LoginErrorTitle, duration: 3000);
				}

				IsLoading = false;
			}
			else
			{
				string errorMsg = string.Empty;

				if (ValidationErrors != null && ValidationErrors.Count > 0)
				{
					errorMsg = ValidationErrors.First().ErrorMessage;
				}
				else
				{
					errorMsg = "An error has occurred.";
				}

				Mvx.Resolve<IUserInteraction>().ToastErrorAlert(errorMsg);
			}
		}

		private void SetWidgetsVisiblity(bool visible)
		{
			IsWidgetVisible = visible;
			IsWidgetHidden = !visible;
		}

		#endregion
	}
}

