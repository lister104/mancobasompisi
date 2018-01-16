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

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel" /> class.
        /// </summary>
        /// <param name="messenger">The messenger.</param>
        /// <param name="userSettings">The user settings.</param>
        /// <param name="auth">The authentication.</param>
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

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Init()
		{           
            DoPingServer();

			SetWidgetsVisiblity(true);

			if(!string.IsNullOrEmpty(_userSettings.Username) && !string.IsNullOrEmpty(_userSettings.Password) && !string.IsNullOrEmpty(_userSettings.AccessCode))
			{
				DoAutoLogin();
			}
		}

        /// <summary>
        /// Does the automatic login.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Does the terms command.
        /// </summary>
        /// <returns></returns>
        public async Task DoTermsCommand()
        {            
            await Mvx.Resolve<IUserInteraction>().AlertAsync("Coming Soon!", "Terms and Conditions", LanguageResolver.Ok);
        }

        /// <summary>
        /// Does the ping server.
        /// </summary>
        /// <returns></returns>
        public async Task DoPingServer()
		{
			await _auth.Ping();
		}

        /// <summary>
        /// Shows the terms.
        /// </summary>
        /// <returns></returns>
        public async Task ShowTerms()
		{
			ShowViewModel<TermsViewModel>();
		}

        /// <summary>
        /// Logins this instance.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Sets the widgets visiblity.
        /// </summary>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        private void SetWidgetsVisiblity(bool visible)
		{
			IsWidgetVisible = visible;
			IsWidgetHidden = !visible;
		}

		#endregion
	}
}

