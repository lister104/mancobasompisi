using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Mancoba.Sompisi.Core.ViewModels;
using Mancoba.Sompisi.Droid.Classes.Helpers;
using Mancoba.Sompisi.Utils.Helpers.UserInteraction;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform;

namespace Mancoba.Sompisi.Droid.Views
{
	[Activity(Label = "Login", Theme = "@style/AppTheme", NoHistory = true)]
	public class LoginView : MvxAppCompatActivity<LoginViewModel>
	{
		public Android.Support.V7.Widget.Toolbar _toolbar;
		public static NavigationView Instance;
		private const string DialogTagName = "TermsDialog";

        /// <summary>
        /// Called when [create].
        /// </summary>
        /// <param name="savedInstanceState">State of the saved instance.</param>
        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.loginview);

			var bindingSet = this.CreateBindingSet<LoginView, LoginViewModel>();
			bindingSet.Apply();

			var versionText = FindViewById<TextView>(Resource.Id.text_version);
			if (versionText != null) versionText.Text = LoginVersion;
		
			var emailaddressEdit = FindViewById<EditText>(Resource.Id.emailaddressEditText);
			var passwordEdit = FindViewById<EditText>(Resource.Id.passwordEditText);			

			var emailaddressLabel = FindViewById<TextView>(Resource.Id.emailaddressLabel);
			var passwordLabel = FindViewById<TextView>(Resource.Id.passwordLabel);			

			var emailaddressUnderline = FindViewById<View>(Resource.Id.emailaddressUnderline);
			var passwordUnderline = FindViewById<View>(Resource.Id.passwordUnderline);			

			emailaddressEdit.FocusChange += (sender, e) =>
			{
				if (!e.HasFocus)
				{
					if (string.IsNullOrEmpty(emailaddressEdit.Text))
					{
						emailaddressLabel.SetTextColor(Colors.Red);
						emailaddressUnderline.SetBackgroundColor(Colors.Red);
						Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.LoginInvalidEmailError);
					}
					else
					{
						emailaddressLabel.SetTextColor(Colors.MediumGray);
						emailaddressUnderline.SetBackgroundColor(Colors.MediumGray);
					}
				}
			};

			passwordEdit.FocusChange += (sender, e) =>
			{
				if (!e.HasFocus)
				{
					if (string.IsNullOrEmpty(passwordEdit.Text))
					{
						passwordLabel.SetTextColor(Colors.Red);
						passwordUnderline.SetBackgroundColor(Colors.Red);
						Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.LoginNoPasswordError);
					}
					else
					{
						passwordLabel.SetTextColor(Colors.MediumGray);
						passwordUnderline.SetBackgroundColor(Colors.MediumGray);
					}
				}
			};
			

			var refresher = FindViewById<ProgressBar>(Resource.Id.progressLogin);
			refresher.IndeterminateDrawable.SetColorFilter(Colors.LightGray, PorterDuff.Mode.Multiply);            

			var appName = FindViewById<TextView>(Resource.Id.appnametext);
			var fontFace = Typeface.CreateFromAsset(BaseContext.Assets, "century-gothic.ttf");
			appName.Typeface = fontFace;
		}

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
				Java.Lang.JavaSystem.Exit(0);

			}
			return base.OnKeyDown(keyCode, e);
		}		

		#region Properties

		private LoginViewModel _viewModel;
		public new LoginViewModel ViewModel => _viewModel ?? (_viewModel = base.ViewModel as LoginViewModel);

        /// <summary>
        /// Gets the login version.
        /// </summary>
        /// <value>
        /// The login version.
        /// </value>
        public string LoginVersion
		{
			get
			{
				var version = PackageManager.GetPackageInfo(PackageName, 0).VersionName + " Build " + PackageManager.GetPackageInfo(PackageName, 0).VersionCode;
				return $"{LanguageResolver.LoginVersion} {version}";
			}
		}

		#endregion
	}
}

