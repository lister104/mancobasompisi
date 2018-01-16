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
using Mancoba.Sompisi.Core.Messages;
using Mancoba.Sompisi.Core.Services.Contracts;
using Mancoba.Sompisi.Core.ViewModels;
using Mancoba.Sompisi.Core.ViewModels.Base;
using Mancoba.Sompisi.Droid.Classes.Caching;
using Mancoba.Sompisi.Droid.Classes.Elements;
using Mancoba.Sompisi.Droid.Classes.Helpers;
using Mancoba.Sompisi.Utils.Helpers;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Shared.Caching;
using MvvmCross.Droid.Shared.Fragments;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Droid.Views
{
    [Activity(Theme = "@style/_MiXNoBar", LaunchMode = LaunchMode.SingleTop, Name = "mancoba.sompisi.droid.views.NavigationView")]
    public class NavigationView : MvxCachingFragmentCompatActivity<NavigationViewModel>
    {
        private DrawerLayout _drawer;
        private MyActionBarDrawerToggle _drawerToggle;
        private string _title;
        private MvxListView _drawerList;
        readonly MvxSubscriptionToken _reachabilityToken;
        private Android.Support.V7.Widget.Toolbar _toolbar;
        readonly MvxSubscriptionToken _navigateToToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationView"/> class.
        /// </summary>
        public NavigationView()
        {
            var messenger = Mvx.Resolve<IMvxMessenger>();
            _reachabilityToken = messenger.Subscribe<ReachabilityMessage>(OnReachabilityChanged);
            _navigateToToken = messenger.Subscribe<NavigateToMessage>(OnNavigateTo);
        }

        private NavigationViewModel _viewModel;

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public new NavigationViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as NavigationViewModel); }
        }

        #region Overrides

        /// <summary>
        /// Called when a key was pressed down and not handled by any of the views
        /// inside of the activity.
        /// </summary>
        /// <param name="keyCode">The value in event.getKeyCode().</param>
        /// <param name="e">Description of the key event.</param>
        /// <returns>
        /// To be added.
        /// </returns>
        /// <remarks>
        /// <para tool="javadoc-to-mdoc">Called when a key was pressed down and not handled by any of the views
        /// inside of the activity. So, for example, key presses while the cursor
        /// is inside a TextView will not trigger the event (unless it is a navigation
        /// to another object) because TextView handles its own key presses.
        /// </para>
        /// <para tool="javadoc-to-mdoc">If the focused view didn't want this event, this method is called.
        /// </para>
        /// <para tool="javadoc-to-mdoc">The default implementation takes care of <c><see cref="F:Android.Views.Keycode.Back" /></c>
        /// by calling <c><see cref="M:Android.App.Activity.OnBackPressed" /></c>, though the behavior varies based
        /// on the application compatibility mode: for
        /// <c><see cref="F:Android.OS.Build.VERSION_CODES.Eclair" /></c> or later applications,
        /// it will set up the dispatch to call <c><see cref="M:Android.App.Activity.OnKeyUp(Android.Views.Keycode, Android.Views.KeyEvent)" /></c> where the action
        /// will be performed; for earlier applications, it will perform the
        /// action immediately in on-down, as those versions of the platform
        /// behaved.
        /// </para>
        /// <para tool="javadoc-to-mdoc">Other additional default key handling may be performed
        /// if configured with <c><see cref="M:Android.App.Activity.SetDefaultKeyMode(Android.App.DefaultKey)" /></c>.</para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/app/Activity.html#onKeyDown(int, android.view.KeyEvent)" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para>
        /// </remarks>
        /// <since version="Added in API level 1" />
        /// <altmember cref="M:Android.App.Activity.OnKeyUp(Android.Views.Keycode, Android.Views.KeyEvent)" />
        /// <altmember cref="T:Android.Views.KeyEvent" />
        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                var currentFragment = SupportFragmentManager.FindFragmentById(Resource.Id.content_frame);
                if (currentFragment != null)
                {

                    if (HandleBackNav(currentFragment))
                    {
                        return true;
                    }
                    else
                    {
                        Java.Lang.JavaSystem.Exit(0);
                    }
                }
                return base.OnKeyDown(keyCode, e);
            }
            return base.OnKeyDown(keyCode, e);
        }

        /// <summary>
        /// Called when [create].
        /// </summary>
        /// <param name="bundle">The bundle.</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.SensorPortrait;

            SetContentView(Resource.Layout.navigationview);

            _title = Title;
            _drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            _drawerList = FindViewById<MvxListView>(Resource.Id.left_drawer);
            _drawerList.Adapter = new CustomAdapter(this, (IMvxAndroidBindingContext)BindingContext, ViewModel);

            _drawer.SetDrawerShadow(Resource.Drawable.drawer_shadow_dark, (int)GravityFlags.Start);
            //DrawerToggle is the animation that happens with the indicator next to the
            //ActionBar icon. You can choose not to use 

            _toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_actionbar);
            _toolbar.SetTitleTextColor(Resource.Color.white);
            SetSupportActionBar(_toolbar);

            SupportActionBar.Title = Title;
            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetDisplayShowCustomEnabled(true);
            _toolbar.SetTitleTextColor(Colors.MyBlue);
            SetMenuIconAndBadge();

            _drawerToggle = new MyActionBarDrawerToggle(this, _drawer,
                                                        //Resource.Drawable.ic_drawer_light,
                                                        Resource.String.navigation_drawer_open,
                                                        Resource.String.navigation_drawer_close);

            //You can alternatively use _drawer.DrawerClosed here
            _drawerToggle.DrawerClosed += delegate
            {
                SupportActionBar.Title = _title;
                InvalidateOptionsMenu();
                //SupportActionBar.Show();
            };

            //You can alternatively use _drawer.DrawerOpened here
            _drawerToggle.DrawerOpened += delegate
            {
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
                SetMenuIconAndBadge();
                InvalidateOptionsMenu();
            };

            _drawer.AddDrawerListener(_drawerToggle);
            
            if (null == bundle)
            {
                if (ViewModel != null)
                {
                    ViewModel.SelectFirstView();
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

        /// <summary>
        /// Called when activity start-up is complete (after <c><see cref="M:Android.App.Activity.OnStart" /></c>
        /// and <c><see cref="M:Android.App.Activity.OnRestoreInstanceState(Android.OS.Bundle)" /></c> have been called).
        /// </summary>
        /// <param name="savedInstanceState">If the activity is being re-initialized after
        /// previously being shut down then this Bundle contains the data it most
        /// recently supplied in <c><see cref="M:Android.App.Activity.OnSaveInstanceState(Android.OS.Bundle)" /></c>.  <format type="text/html"><b><i>Note: Otherwise it is null.</i></b></format></param>
        /// <remarks>
        /// <para tool="javadoc-to-mdoc">Called when activity start-up is complete (after <c><see cref="M:Android.App.Activity.OnStart" /></c>
        /// and <c><see cref="M:Android.App.Activity.OnRestoreInstanceState(Android.OS.Bundle)" /></c> have been called).  Applications will
        /// generally not implement this method; it is intended for system
        /// classes to do final initialization after application code has run.
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <i>Derived classes must call through to the super class's
        /// implementation of this method.  If they do not, an exception will be
        /// thrown.</i>
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/app/Activity.html#onPostCreate(android.os.Bundle)" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para>
        /// </remarks>
        /// <since version="Added in API level 1" />
        /// <altmember cref="M:Android.App.Activity.OnCreate(Android.OS.Bundle)" />
        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            _drawerToggle.SyncState();
        }

        /// <summary>
        /// Called by the system when the device configuration changes while your
        /// activity is running.
        /// </summary>
        /// <param name="newConfig">The new device configuration.</param>
        /// <remarks>
        /// <para tool="javadoc-to-mdoc">Called by the system when the device configuration changes while your
        /// activity is running.  Note that this will <i>only</i> be called if
        /// you have selected configurations you would like to handle with the
        /// <c><see cref="F:Android.Resource.Attribute.ConfigChanges" /></c> attribute in your manifest.  If
        /// any configuration change occurs that is not selected to be reported
        /// by that attribute, then instead of reporting it the system will stop
        /// and restart the activity (to have it launched with the new
        /// configuration).
        /// </para>
        /// <para tool="javadoc-to-mdoc">At the time that this function has been called, your Resources
        /// object will have been updated to return resource values matching the
        /// new configuration.</para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/app/Activity.html#onConfigurationChanged(android.content.res.Configuration)" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para>
        /// </remarks>
        /// <since version="Added in API level 1" />
        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            _drawerToggle.OnConfigurationChanged(newConfig);
        }

        /// <summary>
        /// Prepare the Screen's standard options menu to be displayed.
        /// </summary>
        /// <param name="menu">The options menu as last shown or first initialized by
        /// onCreateOptionsMenu().</param>
        /// <returns>
        /// To be added.
        /// </returns>
        /// <remarks>
        /// <para tool="javadoc-to-mdoc">Prepare the Screen's standard options menu to be displayed.  This is
        /// called right before the menu is shown, every time it is shown.  You can
        /// use this method to efficiently enable/disable items or otherwise
        /// dynamically modify the contents.
        /// </para>
        /// <para tool="javadoc-to-mdoc">The default implementation updates the system menu items based on the
        /// activity's state.  Deriving classes should always call through to the
        /// base class implementation.</para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/app/Activity.html#onPrepareOptionsMenu(android.view.Menu)" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para>
        /// </remarks>
        /// <since version="Added in API level 1" />
        /// <altmember cref="M:Android.App.Activity.OnCreateOptionsMenu(Android.Views.IMenu)" />
        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            var drawerOpen = _drawer.IsDrawerOpen(_drawerList);
            //when open don't show anything
            //			int size = menu.Size ();
            for (int i = 0; i < menu.Size(); i++)
                menu.GetItem(i).SetVisible(!drawerOpen);

            return base.OnPrepareOptionsMenu(menu);
        }

        /// <summary>
        /// This hook is called whenever an item in your options menu is selected.
        /// </summary>
        /// <param name="item">The menu item that was selected.</param>
        /// <returns>
        /// To be added.
        /// </returns>
        /// <remarks>
        /// <para tool="javadoc-to-mdoc">This hook is called whenever an item in your options menu is selected.
        /// The default implementation simply returns false to have the normal
        /// processing happen (calling the item's Runnable or sending a message to
        /// its Handler as appropriate).  You can use this method for any items
        /// for which you would like to do processing without those other
        /// facilities.
        /// </para>
        /// <para tool="javadoc-to-mdoc">Derived classes should call through to the base class for it to
        /// perform the default menu handling.</para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/app/Activity.html#onOptionsItemSelected(android.view.MenuItem)" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para></remarks>
        /// <since version="Added in API level 1" />
        /// <altmember cref="M:Android.App.Activity.OnCreateOptionsMenu(Android.Views.IMenu)" />
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var fragment = SupportFragmentManager.FindFragmentById(Resource.Id.content_frame);

            if (item.ItemId == Resource.Id.action_selections)
                return false;

            bool handeled = HandleBackNav(fragment);

            if (!handeled)
            {
                if (_drawerToggle.OnOptionsItemSelected(item))
                    return true;

                return base.OnOptionsItemSelected(item);
            }
            else
            {
                return true;
            }
            //return true;
        }

        /// <summary>
        /// Called when [resume].
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();

            if (!RunningMode.IsInForeground)
                Mvx.Resolve<IMobileDataService>().ForceFetch = true;

            RunningMode.IsInForeground = true;
        }

        /// <summary>
        /// Called when [pause].
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
            RunningMode.IsInForeground = false;
        }

        /// <summary>
        /// Called when [reachability changed].
        /// </summary>
        /// <param name="obj">The object.</param>
        void OnReachabilityChanged(ReachabilityMessage obj)
        {
            bool info = (bool)obj.Sender;
            string internet = info ? LanguageResolver.InternetConnected : LanguageResolver.NoInternet;
            var toast = Toast.MakeText(this, internet, ToastLength.Short);
            toast.SetGravity(GravityFlags.Top | GravityFlags.Center, 0, 100);
            toast.Show();
        }

        /// <summary>
        /// Called when [navigate to].
        /// </summary>
        /// <param name="obj">The object.</param>
        void OnNavigateTo(NavigateToMessage obj)
        {
            SupportFragmentManager.PopBackStack(null, (int)PopBackStackFlags.None);
            ViewModel.SelectFirstView();
        }

        #endregion

        #region Fragments

        /// <summary>
        /// Builds the fragment cache configuration.
        /// </summary>
        /// <returns></returns>
        public override IFragmentCacheConfiguration BuildFragmentCacheConfiguration()
        {
            var frag = new FragmentCacheConfigurationCustomFragmentInfo(); // custom FragmentCacheConfiguration is used because custom IMvxFragmentInfo is used -> CustomFragmentInfo
            return frag;
        }

        /// <summary>
        /// Shoulds the replace current fragment.
        /// </summary>
        /// <param name="newFragment">The new fragment.</param>
        /// <param name="currentFragment">The current fragment.</param>
        /// <param name="replacementBundle">The replacement bundle.</param>
        /// <returns></returns>
        protected override FragmentReplaceMode ShouldReplaceCurrentFragment(IMvxCachedFragmentInfo newFragment, IMvxCachedFragmentInfo currentFragment, Bundle replacementBundle)
        {
            var fragInfo = newFragment as CustomFragmentInfo;

            newFragment.ContentId = Resource.Id.content_frame;

            var current = SupportFragmentManager.FindFragmentById(Resource.Id.adminid);
            var transaction = SupportFragmentManager.BeginTransaction();

            newFragment.CachedFragment = Android.Support.V4.App.Fragment.Instantiate(this, FragmentJavaName(newFragment.FragmentType), replacementBundle) as IMvxFragmentView;

            var cache = Mvx.GetSingleton<IMvxMultipleViewModelCache>();

            cache.GetAndClear(newFragment.ViewModelType, GetTagFromFragment(newFragment.CachedFragment as Android.Support.V4.App.Fragment));

            try
            {
                if (fragInfo.IsRoot)
                {

                    SupportFragmentManager.PopBackStack(null, (int)PopBackStackFlags.Inclusive);
                    transaction.Replace(newFragment.ContentId, (Android.Support.V4.App.Fragment)newFragment.CachedFragment, newFragment.Tag);
                }
                else
                {
                    transaction.Replace(newFragment.ContentId, (Android.Support.V4.App.Fragment)newFragment.CachedFragment, newFragment.Tag)
                                                   .AddToBackStack(null);
                }

                //transaction.Add (newFragment.ContentId, (Android.Support.V4.App.Fragment)newFragment.CachedFragment, newFragment.Tag)
                // .AddToBackStack (null);
                //if (current != null)
                //	transaction.Hide (current);

                transaction.Commit();
                Show(fragInfo);
            }
            catch (Exception ex)
            {

            }
            return FragmentReplaceMode.NoReplace;
        }

        /// <summary>
        /// Closes the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        public override bool Close(IMvxViewModel viewModel)
        {
            //if (viewModel is ServersViewModel) {
            //	if ((viewModel as ServersViewModel).ServerHasChanged) {
            //		FragmentManager.PopBackStack (null, PopBackStackFlags.Inclusive);
            //		Finish ();
            //	}
            //	return;
            //}
            var currentFragment = SupportFragmentManager.FindFragmentById(Resource.Id.content_frame);
            HandleBackNav(currentFragment);
            return true;
        }

        /// <summary>
        /// Shows the specified my custom information.
        /// </summary>
        /// <param name="myCustomInfo">My custom information.</param>
        /// <returns></returns>
        public bool Show(CustomFragmentInfo myCustomInfo)
        {
            try
            {
                var title = string.Empty;
                var section = ViewModel.GetSectionForViewModelType(myCustomInfo.ViewModelType);

                switch (section)
                {
                    case Section.Drafts:
                        {
                            title = LanguageResolver.Drafts.ToUpperInvariant();
                            SetMenuIconAndBadge();
                        }
                        break;
                    case Section.Outbox:
                        {
                            title = LanguageResolver.Outbox.ToUpperInvariant();
                            //PreviousTitle = SupportActionBar.Title;
                            SetMenuIconAndBadge();
                        }
                        break;
                    case Section.Sent:
                        {
                            title = LanguageResolver.Sent.ToUpperInvariant();
                            SetMenuIconAndBadge();
                        }
                        break;

                    case Section.Settings:
                        {
                            title = LanguageResolver.MenuSettings.ToUpperInvariant();
                            SetMenuIconAndBadge();
                        }
                        break;
                }

                _drawerList.SetItemChecked(ViewModel.MenuItems.FindIndex(m => m.Id == (int)section), true);
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

        /// <summary>
        /// Handles the back nav.
        /// </summary>
        /// <param name="fragment">The fragment.</param>
        /// <returns></returns>
        bool HandleBackNav(Android.Support.V4.App.Fragment fragment)
        {
            SupportFragmentManager.PopBackStack(null, (int)PopBackStackFlags.None);

            if (!string.IsNullOrEmpty(PreviousTitle))
            {
                SupportActionBar.Title = _title = PreviousTitle;
                PreviousTitle = string.Empty;
            }

            return false;
        }

        /// <summary>
        /// Sets the menu icon and badge.
        /// </summary>
        /// <param name="backArrow">if set to <c>true</c> [back arrow].</param>
        /// <param name="fromBackgroundService">if set to <c>true</c> [from background service].</param>
        public void SetMenuIconAndBadge(bool backArrow = false, bool fromBackgroundService = false)
        {
            if (backArrow)
            {
                RunOnUiThread(() => _toolbar.NavigationIcon = ContextCompat.GetDrawable(this, Resource.Drawable.ic_arrow_back));
                return;
            }
            RunOnUiThread(() => _toolbar.NavigationIcon = ContextCompat.GetDrawable(this, Resource.Drawable.menu));
        }

        #endregion

        #region Properties

        public Type ViewModelType { get { return typeof(NavigationViewModel); } }

        public string PreviousTitle { get; set; }

        #endregion

        #region Custom Adapter

        public class CustomAdapter : MvxAdapter
        {
            readonly NavigationViewModel _navModel;
            readonly Context _context;

            /// <summary>
            /// Initializes a new instance of the <see cref="CustomAdapter"/> class.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="bindingContext">The binding context.</param>
            /// <param name="navModel">The nav model.</param>
            public CustomAdapter(Context context, IMvxAndroidBindingContext bindingContext, NavigationViewModel navModel)
                : base(context, bindingContext)
            {
                _navModel = navModel;
                _context = context;
            }

            /// <summary>
            /// Get the type of View that will be created by <c><see cref="!:Android.Widget.Adapter.getView(int, android.view.View, android.view.ViewGroup)" /></c> for the specified item.
            /// </summary>
            /// <param name="position">The position of the item within the adapter's data set whose view type we
            /// want.</param>
            /// <returns>
            /// To be added.
            /// </returns>
            /// <remarks>
            /// <para tool="javadoc-to-mdoc">Get the type of View that will be created by <c><see cref="!:Android.Widget.Adapter.getView(int, android.view.View, android.view.ViewGroup)" /></c> for the specified item.</para>
            /// <para tool="javadoc-to-mdoc">
            ///   <format type="text/html">
            ///     <a href="http://developer.android.com/reference/android/widget/BaseAdapter.html#getItemViewType(int)" target="_blank">[Android Documentation]</a>
            ///   </format>
            /// </para>
            /// </remarks>
            /// <since version="Added in API level 1" />
            public override int GetItemViewType(int position)
            {
                GetRawItem(position);
                return 1;
            }

            /// <summary>
            /// </summary>
            /// <value>
            /// To be added.
            /// </value>
            /// <remarks>
            /// <para tool="javadoc-to-mdoc" />
            /// <para tool="javadoc-to-mdoc">
            /// Returns the number of types of Views that will be created by
            /// <c><see cref="!:Android.Widget.Adapter.getView(int, android.view.View, android.view.ViewGroup)" /></c>. Each type represents a set of views that can be
            /// converted in <c><see cref="!:Android.Widget.Adapter.getView(int, android.view.View, android.view.ViewGroup)" /></c>. If the adapter always returns the same
            /// type of View for all items, this method should return 1.
            /// </para>
            /// <para tool="javadoc-to-mdoc">
            /// This method will only be called when when the adapter is set on the
            /// the <c><see cref="T:Android.Widget.AdapterView" /></c>.
            /// </para>
            /// <para tool="javadoc-to-mdoc">
            ///   <format type="text/html">
            ///     <a href="http://developer.android.com/reference/android/widget/BaseAdapter.html#getViewTypeCount()" target="_blank">[Android Documentation]</a>
            ///   </format>
            /// </para>
            /// </remarks>
            /// <since version="Added in API level 1" />
            public override int ViewTypeCount
            {
                get { return 1; }
            }

            /// <summary>
            /// Gets the bindable view.
            /// </summary>
            /// <param name="convertView">The convert view.</param>
            /// <param name="source">The source.</param>
            /// <param name="templateId">The template identifier.</param>
            /// <returns></returns>
            protected override View GetBindableView(View convertView, object source, int templateId)
            {
                MenuViewModel menu = source as MenuViewModel;

                View view = base.GetBindableView(convertView, source, templateId);
                var imageView = view.FindViewById<ImageView>(Resource.Id.menuimage);
                var textView = view.FindViewById<TextView>(Resource.Id.menutext);

                textView.SetTextColor(Android.Graphics.Color.White);

                if (menu == null) return view;

                switch (menu.Section)
                {
                    case Section.Drafts:
                        imageView.SetImageResource(Resource.Drawable.ic_mydetails);
                        break;

                    case Section.Outbox:
                        imageView.SetImageResource(Resource.Drawable.ic_installers);
                        break;

                    case Section.Sent:
                        imageView.SetImageResource(Resource.Drawable.ic_favorite);
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

                LinearLayout linear = view.FindViewById<LinearLayout>(Resource.Id.menuitem);
                ViewGroup.LayoutParams layout = linear.LayoutParameters;
                if (menu.MenuIndex == 0)
                    //					if (string.IsNullOrEmpty(menu.Title))
                    layout.Height = 0;
                //					else
                //						layout.Height = -1;

                linear.LayoutParameters = layout;

                return view;
            }

            /// <summary>
            /// Returns true if the item at the specified position is not a separator.
            /// </summary>
            /// <param name="position">Index of the item</param>
            /// <returns>
            /// To be added.
            /// </returns>
            /// <remarks>
            /// <para tool="javadoc-to-mdoc">Returns true if the item at the specified position is not a separator.
            /// (A separator is a non-selectable, non-clickable item).
            /// The result is unspecified if position is invalid. An <c><see cref="T:Java.Lang.ArrayIndexOutOfBoundsException" /></c>
            /// should be thrown in that case for fast failure.</para>
            /// <para tool="javadoc-to-mdoc">
            ///   <format type="text/html">
            ///     <a href="http://developer.android.com/reference/android/widget/BaseAdapter.html#isEnabled(int)" target="_blank">[Android Documentation]</a>
            ///   </format>
            /// </para>
            /// </remarks>
            /// <since version="Added in API level 1" />
            public override bool IsEnabled(int position)
            {
                //MenuViewModel model = _navModel.GetMenuItemForMainMenuIndex(position);
                //return model.Section != Section.Unknown;
                return true;
            }
        }

        #endregion
    }
}

