using System.Linq;
using System.Threading.Tasks;
using Mancoba.Sompisi.Core.Services.Contracts;
using Mancoba.Sompisi.Core.Validators;
using Mancoba.Sompisi.Core.ViewModels.Base;
using Mancoba.Sompisi.Utils.Helpers.UserInteraction;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.ViewModels
{
	public class InstallerSearchViewModel : BaseValidationViewModel
	{
		#region Private Variables	
		private readonly InstallerSearchValidator _validator;
		private readonly IMobileDataService _dataService;
		private string _searchText;
		private bool _isWidgetHidden;
		private bool _isWidgetVisible;

		#endregion

		#region Public Properties
		public string SearchTextPlaceholder => LanguageResolver.SearchText;

		public string SearchButtonTitle => LanguageResolver.SearchButton;

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

		public string SearchText
		{
			get { return _searchText; }
			set
			{
				_searchText = value.Trim();
				RaisePropertyChanged(() => SearchText);

			}
		}

		public IMvxAsyncCommand SearchCommand { get; }

		#endregion

		#region Public Methods

		public void Init()
		{			
			SetWidgetsVisiblity(true);
		}
		public InstallerSearchViewModel(IMvxMessenger messenger, IMobileDataService dataService) : base(messenger)
		{
			_dataService = dataService;
			SearchCommand = new MvxAsyncCommand(SearchCommandHandler);
			_validator = new InstallerSearchValidator();
		}

		public async Task SearchCommandHandler()
		{
			ValidationErrors?.Clear();

			ValidateModel(_validator, this);

			if (IsModelValid)
			{
				IsLoading = true;

				var installers = await _dataService.FindInstallers(SearchText);

				if (installers != null && installers.Count > 0)
				{
					ShowViewModel<InstallerListViewModel>(installers);
				}
				else
				{
					await Mvx.Resolve<IUserInteraction>().AlertAsync(LanguageResolver.SearchErrorTitle, "No Customers Found");
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
