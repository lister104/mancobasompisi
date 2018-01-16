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

