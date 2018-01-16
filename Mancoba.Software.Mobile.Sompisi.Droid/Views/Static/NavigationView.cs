using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Bluescore.DStv.Core.Messages;
using Bluescore.DStv.Core.Services.Contracts;
using Bluescore.DStv.Core.ViewModels;
using Bluescore.DStv.Core.ViewModels.Base;
using Bluescore.DStv.Droid.Classes.Caching;
using Bluescore.DStv.Droid.Classes.Elements;
using Bluescore.DStv.Droid.Classes.Helpers;
using Bluescore.DStv.Utils.Helpers;
using Bluescore.DStv.Utils.Language;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Shared.Caching;
using MvvmCross.Droid.Shared.Fragments;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Bluescore.DStv.Droid.Views
{
	[Activity (Theme = "@style/_MiXNoBar", LaunchMode = LaunchMode.SingleTop, Name = "bluescore.tv.droid.views.NavigationView")]
	public class NavigationView : MvxCachingFragmentCompatActivity<NavigationViewModel>
	{
		private DrawerLayout _drawer;
		private MyActionBarDrawerToggle _drawerToggle;
		private string _title;
		private MvxListView _drawerList;
		readonly MvxSubscriptionToken _reachabilityToken;
		private Android.Support.V7.Widget.Toolbar _toolbar;
		readonly MvxSubscriptionToken _navigateToToken;
		string _orgName;

		public NavigationView ()
		{
			var messenger = Mvx.Resolve<IMvxMessenger> ();
			_reachabilityToken = messenger.Subscribe<ReachabilityMessage> (OnReachabilityChanged);
			_navigateToToken = messenger.Subscribe<NavigateToMessage> (OnNavigateTo);

		}
		private NavigationViewModel _viewModel;

		public new NavigationViewModel ViewModel {
			get { return _viewModel ?? (_viewModel = base.ViewModel as NavigationViewModel); }
		}


		#region Overrides
		public override bool OnKeyDown (Keycode keyCode, KeyEvent e)
		{
			if (keyCode == Keycode.Back) {
				var currentFragment = SupportFragmentManager.FindFragmentById (Resource.Id.content_frame);
				if (currentFragment != null) {

					if (currentFragment is AcceptPaymentView) {

						(currentFragment as AcceptPaymentView).ViewModel.SwitchEditMode ();
						return true;

					}

					if (HandleBackNav (currentFragment)) {
						return true;
					} else {
						Java.Lang.JavaSystem.Exit (0);
					}
				}

				return base.OnKeyDown (keyCode, e);
			}

			return base.OnKeyDown (keyCode, e);
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			this.RequestedOrientation = Android.Content.PM.ScreenOrientation.SensorPortrait;

			SetContentView (Resource.Layout.navigationview);

			_title = Title;
			_drawer = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			_drawerList = FindViewById<MvxListView> (Resource.Id.left_drawer);
			_drawerList.Adapter = new CustomAdapter (this, (IMvxAndroidBindingContext)BindingContext, ViewModel);

			_drawer.SetDrawerShadow (Resource.Drawable.drawer_shadow_dark, (int)GravityFlags.Start);
			//DrawerToggle is the animation that happens with the indicator next to the
			//ActionBar icon. You can choose not to use 

			_toolbar = FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.toolbar_actionbar);
			_toolbar.SetTitleTextColor (Resource.Color.white);
			SetSupportActionBar (_toolbar);

			SupportActionBar.Title = Title;
			SupportActionBar.SetDisplayHomeAsUpEnabled (false);
			SupportActionBar.SetDisplayShowHomeEnabled (true);
			SupportActionBar.SetDisplayShowCustomEnabled (true);
			_toolbar.SetTitleTextColor (Colors.MiXGreen);
			SetMenuIconAndBadge ();


			_drawerToggle = new MyActionBarDrawerToggle (this, _drawer,
														//Resource.Drawable.ic_drawer_light,
														Resource.String.navigation_drawer_open,
														Resource.String.navigation_drawer_close);


			//You can alternatively use _drawer.DrawerClosed here
			_drawerToggle.DrawerClosed += delegate {
				SupportActionBar.Title = _title;
				InvalidateOptionsMenu ();
				//				SupportActionBar.Show();
			};

			//You can alternatively use _drawer.DrawerOpened here
			_drawerToggle.DrawerOpened += delegate {
				//_orgName = "";
				//_org = Mvx.Resolve<IOrganisationManager> ();
				//if (_org != null) {
				//	var selectedOrg = _org.LoadSelectedOrganisation();
				//	if(selectedOrg != null)
				//	{
				//		_orgName = selectedOrg.Name;
				//	}
				//}
				//SupportActionBar.Title = _orgName.ToUpper();
				//				SupportActionBar.Hide();
				SetMenuIconAndBadge ();
				InvalidateOptionsMenu ();
			};

			_drawer.AddDrawerListener (_drawerToggle);



			if (null == bundle) {
				if (ViewModel != null) {
					ViewModel.SelectFirstView ();
				}
			}

			//try {

			//Regsiter with API
			//				GcmClient.CheckDevice(this);
			//				GcmClient.CheckManifest(this);
			//
			//				if(string.IsNullOrEmpty(Mvx.Resolve<IUserSettings>().DeviceToken))
			//				{
			//					GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);
			//				}
			//}
			//catch(Exception ex) {
			//	//RaygunClient.Current.Send (ex);
			//}
		}



		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			_drawerToggle.SyncState ();
		}

		public override void OnConfigurationChanged (Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			_drawerToggle.OnConfigurationChanged (newConfig);
		}

		public override bool OnPrepareOptionsMenu (IMenu menu)
		{
			var drawerOpen = _drawer.IsDrawerOpen (_drawerList);
			//when open don't show anything
			//			int size = menu.Size ();
			for (int i = 0; i < menu.Size (); i++)
				menu.GetItem (i).SetVisible (!drawerOpen);

			return base.OnPrepareOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{

			var fragment = SupportFragmentManager.FindFragmentById (Resource.Id.content_frame);

			if (item.ItemId == Resource.Id.action_selections)
				return false;


			bool handeled = HandleBackNav (fragment);

			if (!handeled) {
				if (_drawerToggle.OnOptionsItemSelected (item))
					return true;

				return base.OnOptionsItemSelected (item);
			} else {
				return true;
			}

			//return true;
		}


		protected override void OnResume ()
		{
			base.OnResume ();

			if (!RunningMode.IsInForeground)
				Mvx.Resolve<IMobileDataService> ().ForceFetch = true;

			RunningMode.IsInForeground = true;
		}
		protected override void OnPause ()
		{
			base.OnPause ();
			RunningMode.IsInForeground = false;
		}

		void OnReachabilityChanged (ReachabilityMessage obj)
		{
			bool info = (bool)obj.Sender;
			string internet = info ? LanguageResolver.InternetConnected : LanguageResolver.NoInternet;
			var toast = Toast.MakeText (this, internet, ToastLength.Short);
			toast.SetGravity (GravityFlags.Top | GravityFlags.Center, 0, 100);
			toast.Show ();

		}

		void OnNavigateTo (NavigateToMessage obj)
		{
			SupportFragmentManager.PopBackStack (null, (int)PopBackStackFlags.None);
			ViewModel.SelectFirstView();
		}

		#endregion

		#region Fragments

		public override IFragmentCacheConfiguration BuildFragmentCacheConfiguration ()
		{
			var frag = new FragmentCacheConfigurationCustomFragmentInfo (); // custom FragmentCacheConfiguration is used because custom IMvxFragmentInfo is used -> CustomFragmentInfo
			return frag;
		}

		protected override FragmentReplaceMode ShouldReplaceCurrentFragment (IMvxCachedFragmentInfo newFragment, IMvxCachedFragmentInfo currentFragment, Bundle replacementBundle)
		{


			var fragInfo = newFragment as CustomFragmentInfo;

			newFragment.ContentId = Resource.Id.content_frame;

			var current = SupportFragmentManager.FindFragmentById (Resource.Id.adminid);
			var transaction = SupportFragmentManager.BeginTransaction ();


			newFragment.CachedFragment = Android.Support.V4.App.Fragment.Instantiate (this, FragmentJavaName (newFragment.FragmentType), replacementBundle) as IMvxFragmentView;

			var cache = Mvx.GetSingleton<IMvxMultipleViewModelCache> ();

			cache.GetAndClear (newFragment.ViewModelType, GetTagFromFragment (newFragment.CachedFragment as Android.Support.V4.App.Fragment));

			try {

				if (fragInfo.IsRoot) {

					SupportFragmentManager.PopBackStack (null, (int)PopBackStackFlags.Inclusive);
					transaction.Replace (newFragment.ContentId, (Android.Support.V4.App.Fragment)newFragment.CachedFragment, newFragment.Tag);

				} else {
					transaction.Replace (newFragment.ContentId, (Android.Support.V4.App.Fragment)newFragment.CachedFragment, newFragment.Tag)
												   .AddToBackStack (null);
				}

				//transaction.Add (newFragment.ContentId, (Android.Support.V4.App.Fragment)newFragment.CachedFragment, newFragment.Tag)
				// .AddToBackStack (null);
				//if (current != null)
				//	transaction.Hide (current);


				transaction.Commit ();
				Show (fragInfo);


			} catch (Exception ex) {

			}



			return FragmentReplaceMode.NoReplace;
		}


		public override bool Close (IMvxViewModel viewModel)
		{
			//if (viewModel is ServersViewModel) {
			//	if ((viewModel as ServersViewModel).ServerHasChanged) {
			//		FragmentManager.PopBackStack (null, PopBackStackFlags.Inclusive);
			//		Finish ();
			//	}
			//	return;
			//}
			var currentFragment = SupportFragmentManager.FindFragmentById (Resource.Id.content_frame);
			HandleBackNav (currentFragment);
			return true;

		}

		public bool Show(CustomFragmentInfo myCustomInfo)
		{
			try
			{

				var title = string.Empty;
				var section = ViewModel.GetSectionForViewModelType(myCustomInfo.ViewModelType);

				switch (section)
				{
					case Section.Search:
					{
						title = LanguageResolver.MenuSearch.ToUpperInvariant();
						SetMenuIconAndBadge();
					}
						break;

					case Section.ProviderList:
					{
						title = LanguageResolver.ListOfCustomers.ToUpperInvariant();
						SetMenuIconAndBadge();
					}
						break;
					case Section.AddCustomer:
					{
						title = LanguageResolver.AddNewCustomer.ToUpperInvariant();
						SetMenuIconAndBadge();
					}
						break;

					case Section.AddAddress:
					{
						title = LanguageResolver.MenuManageAddresses.ToUpperInvariant();
						SetMenuIconAndBadge();
					}
						break;

					case Section.AcceptPayment:
					{
						title = LanguageResolver.AcceptAPayment.ToUpperInvariant();
						SetMenuIconAndBadge();
					}
						break;
					case Section.Settings:
					{
						title = LanguageResolver.MenuSettings.ToUpperInvariant();
						SetMenuIconAndBadge();
					}
						break;
					case Section.ProviderDetails:
					{
						title = LanguageResolver.CustomerDetails.ToUpperInvariant();
						PreviousTitle = SupportActionBar.Title;

					}
						break;					
					case Section.AssetInstallSummary:
					{
						title = "NEW INSTALLATION";
						PreviousTitle = SupportActionBar.Title;
					}
						break;

				}

				_drawerList.SetItemChecked(ViewModel.MenuItems.FindIndex(m => m.Id == (int) section), true);
				SupportActionBar.Title = _title = title;

				return true;
			}
			finally
			{
				_drawer.CloseDrawer(_drawerList);
			}
		}


		#endregion


		#region Private Methods

		bool HandleBackNav (Android.Support.V4.App.Fragment fragment)
		{

			SupportFragmentManager.PopBackStack (null, (int)PopBackStackFlags.None);

			if(!string.IsNullOrEmpty(PreviousTitle))
			{
				SupportActionBar.Title = _title = PreviousTitle;
				PreviousTitle = string.Empty;
			}

			return false;
		}

		public void SetMenuIconAndBadge (bool backArrow = false, bool fromBackgroundService = false)
		{
			if (backArrow) {

				RunOnUiThread (() => _toolbar.NavigationIcon = ContextCompat.GetDrawable (this, Resource.Drawable.ic_arrow_back));
				return;
			}

			RunOnUiThread (() => _toolbar.NavigationIcon = ContextCompat.GetDrawable (this, Resource.Drawable.ic_menu));

		}

		#endregion

		#region Properties

		public Type ViewModelType { get { return typeof (NavigationViewModel); } }

		public string PreviousTitle { get; set; }

		#endregion

		#region Custom Adapter

		public class CustomAdapter : MvxAdapter
		{
			readonly NavigationViewModel _navModel;
			readonly Context _context;

			public CustomAdapter (Context context, IMvxAndroidBindingContext bindingContext, NavigationViewModel navModel)
				: base (context, bindingContext)
			{
				_navModel = navModel;
				_context = context;
			}

			public override int GetItemViewType (int position)
			{
				GetRawItem (position);
				return 1;
			}

			public override int ViewTypeCount {
				get { return 1; }
			}

			protected override View GetBindableView (View convertView, object source, int templateId)
			{
				MenuViewModel menu = source as MenuViewModel;

				View view = base.GetBindableView (convertView, source, templateId);
				var imageView = view.FindViewById<ImageView> (Resource.Id.menuimage);
				var textView = view.FindViewById<TextView> (Resource.Id.menutext);

				textView.SetTextColor (Android.Graphics.Color.White);

				if (menu == null) return view;

				switch (menu.Section)
				{
					case Section.Search:
						imageView.SetImageResource(Resource.Drawable.ic_selections_green);
						break;

					case Section.AddCustomer:
						imageView.SetImageResource(Resource.Drawable.ic_personal_green);
						break;

					case Section.AcceptPayment:
						imageView.SetImageResource(Resource.Drawable.ic_myworkorders);
						break;

                    case Section.AddAddress:
                        imageView.SetImageResource(Resource.Drawable.ic_selections_green);
                        break;

                    case Section.ProviderList:
						imageView.SetImageResource(Resource.Drawable.ic_workorders);
						break;
					
					case Section.Settings:
						imageView.SetImageResource(Resource.Drawable.ic_settings);
						break;

					case Section.Logout:
						imageView.SetImageResource(Resource.Drawable.ic_logout);
						break;
					//				case Section.Notifications:
					//					imageView.SetImageResource(Resource.Drawable.ic_notifications);
					//					break;
					//				case Section.NotificationRules:
					//					imageView.SetImageResource(Resource.Drawable.ic_notifications_settings);
					//					break;

				}

				LinearLayout linear = view.FindViewById<LinearLayout> (Resource.Id.menuitem);
				ViewGroup.LayoutParams layout = linear.LayoutParameters;
				if (menu.MenuIndex == 0)
					//					if (string.IsNullOrEmpty(menu.Title))
					layout.Height = 0;
				//					else
				//						layout.Height = -1;

				linear.LayoutParameters = layout;

				return view;
			}

			public override bool IsEnabled (int position)
			{
				//MenuViewModel model = _navModel.GetMenuItemForMainMenuIndex(position);
				//return model.Section != Section.Unknown;
				return true;
			}
		}

		#endregion


	}
}

