using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mancoba.Sompisi.Utils.Interfaces;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.ViewModels.Base
{	
	public enum Section
	{
		Unknown,
        Drafts,
        Outbox,
        Sent,
        Settings,				
		Terms,
		Logout
	}

	public class NavigationViewModel : MessengerBaseViewModel
	{		
		public NavigationViewModel(IMvxMessenger messenger)
			: base(messenger)
		{
			LoadMenu();
		}

		private List<MenuViewModel> _menuItems;

		public List<MenuViewModel> MenuItems
		{
			get
			{
				return _menuItems.Where(x => x.IsOnMainMenu).ToList();
			}
			set
			{
				_menuItems = value;
				RaisePropertyChanged(() => MenuItems);
			}
		}

		private void LoadMenu()
		{
			_menuItems = new List<MenuViewModel>
			{
				new MenuViewModel
				{
					Section = Section.Unknown,

					ViewModelType = null,
					IsOnMainMenu = true,
					MenuIndex = 0
				},
				new MenuViewModel
				{
					Section = Section.Drafts,
					Title = LanguageResolver.MenuDrafts,
					ViewModelType = typeof(DraftsViewModel),
					IsOnMainMenu = true,
					MenuIndex = 1
				},
				new MenuViewModel
				{
					Section = Section.Outbox,
					Title = LanguageResolver.MenuOutbox,
					ViewModelType = typeof(OutboxViewModel),
					IsOnMainMenu = true,
					MenuIndex = 2
				},
				new MenuViewModel
				{
					Section = Section.Sent,
					Title = LanguageResolver.MenuSent,
					ViewModelType = typeof(SentListViewModel),
					IsOnMainMenu = true,
					MenuIndex = 3
				},                
    //            new MenuViewModel
				//{
				//	Section = Section.Settings,
				//	Title = LanguageResolver.MenuSettings ,
				//	ViewModelType = typeof(SettingsViewModel),
				//	IsOnMainMenu = false,
				//	MenuIndex = 4
				//},
				new MenuViewModel
				{
					Section = Section.Logout,
					Title = LanguageResolver.MenuLogout,
					ViewModelType = typeof(LoginViewModel),                    
					IsOnMainMenu = true,
					MenuIndex = 4
				},
			};
			RaisePropertyChanged(() => MenuItems);
		}

		private MvxAsyncCommand<MenuViewModel> _selectMenuItemCommand;

		public IMvxAsyncCommand SelectMenuItemCommand
		{
			get
			{
				return _selectMenuItemCommand ?? (_selectMenuItemCommand = new MvxAsyncCommand<MenuViewModel>(ExecuteSelectMenuItemCommand));
			}
		}

	    private async Task ExecuteSelectMenuItemCommand(MenuViewModel item)
	    {
	        await Task.Run(() =>
	        {
	            //navigate if we have to, pass the id so we can grab from cache... or not
	            switch (item.Section)
	            {
	                case Section.Drafts:
	                    ShowViewModel<DraftsViewModel>();
	                    break;

	                case Section.Outbox:
	                    ShowViewModel<OutboxViewModel>();
	                    break;

	                case Section.Sent:
	                    ShowViewModel<SentListViewModel>();
	                    break;
                        
	                case Section.Settings:
	                    ShowViewModel<SettingsViewModel>();
	                    break;

	                case Section.Terms:
	                    ShowViewModel<TermsViewModel>();
	                    break;
	                case Section.Logout:
	                    IUserSettings settings = Mvx.Resolve<IUserSettings>();
	                    settings.Clear();
	                    ShowViewModel<LoginViewModel>(0);
	                    Close(this);
	                    break;
	            }
	        });
	    }

	    public bool IsChildViewForViewModelType(Type type)
		{
			if (type == typeof(TermsViewModel))
				return true;
			
			return false;
		}

		public bool IsFirstLevelViewForViewModelType(Type type)
		{			
			if (type == typeof(DraftsViewModel))
				return true;

            if (type == typeof(OutboxViewModel))
                return true;

            if (type == typeof(SentListViewModel))
                return true;

            if (type == typeof(SentViewModel))
                return true;
            
            if (type == typeof(SettingsViewModel))
				return true;

			if (type == typeof(TermsViewModel))
				return true;			

			return false;
		}

		public Section GetSectionForViewModelType(Type type)
		{
            if (type == typeof(DraftsViewModel))
                return Section.Drafts;

            if (type == typeof(OutboxViewModel))
                return Section.Outbox;

            if (type == typeof(SentListViewModel))
                return Section.Sent;

            //if (type == typeof(SentViewModel))
            //    return Section.Sent;
            
            if (type == typeof(SettingsViewModel))
                return Section.Settings;

            if (type == typeof(TermsViewModel))
                return Section.Terms;            

			return Section.Drafts;
		}

		#region Properties		

		public int ToolbarHeight { get; set; }

		#endregion

		#region Methods

		public int GetIndexOfMainMenuItem(Type type)
		{
			var i = MenuItems.Where(m => m.IsOnMainMenu && m.ViewModelType == type).Select(m => m.MenuIndex).FirstOrDefault();
			return i;
		}

		public MenuViewModel GetMenuItemForMainMenuIndex(int index)
		{
			var i = MenuItems.Where(m => m.IsOnMainMenu && m.MenuIndex == index).Select(m => m).FirstOrDefault();
			return i;

		}

		/// <summary>
		/// Selects the first view to show on app startup from navigation menu
		/// </summary>
		public void SelectFirstView()
		{			
			SelectMenuItemCommand.Execute(MenuItems.FirstOrDefault(vm => vm.Section == Section.Drafts));
		}

		/// <summary>
		/// Selects the first view to show on app startup from navigation menu
		/// </summary>
		public void SetUpForTablet()
		{
		    SelectFirstView();
		}

		public bool UseFragManager { get; set; }

		#endregion
	}
}

