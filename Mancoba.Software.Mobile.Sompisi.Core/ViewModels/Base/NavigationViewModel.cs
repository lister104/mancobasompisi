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
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationViewModel"/> class.
        /// </summary>
        /// <param name="messenger">The messenger.</param>
        public NavigationViewModel(IMvxMessenger messenger)
			: base(messenger)
		{
			LoadMenu();
		}

		private List<MenuViewModel> _menuItems;

        /// <summary>
        /// Gets or sets the menu items.
        /// </summary>
        /// <value>
        /// The menu items.
        /// </value>
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

        /// <summary>
        /// Loads the menu.
        /// </summary>
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

        /// <summary>
        /// Gets the select menu item command.
        /// </summary>
        /// <value>
        /// The select menu item command.
        /// </value>
        public IMvxAsyncCommand SelectMenuItemCommand
		{
			get
			{
				return _selectMenuItemCommand ?? (_selectMenuItemCommand = new MvxAsyncCommand<MenuViewModel>(ExecuteSelectMenuItemCommand));
			}
		}

        /// <summary>
        /// Executes the select menu item command.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Determines whether [is child view for view model type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is child view for view model type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsChildViewForViewModelType(Type type)
		{
			if (type == typeof(TermsViewModel))
				return true;
			
			return false;
		}

        /// <summary>
        /// Determines whether [is first level view for view model type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is first level view for view model type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Gets the type of the section for view model.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the index of main menu item.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public int GetIndexOfMainMenuItem(Type type)
		{
			var i = MenuItems.Where(m => m.IsOnMainMenu && m.ViewModelType == type).Select(m => m.MenuIndex).FirstOrDefault();
			return i;
		}

        /// <summary>
        /// Gets the index of the menu item for main menu.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
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

