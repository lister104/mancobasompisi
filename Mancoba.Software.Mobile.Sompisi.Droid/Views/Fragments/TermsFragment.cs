using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
using Android.Views;
using Android.Views.InputMethods;
using Android.Webkit;
using Mancoba.Sompisi.Core.ViewModels;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;

namespace Mancoba.Sompisi.Droid.Views.Fragments
{
	[Activity(Label = "Terms & Conditions", Theme = "@style/AppTheme")]
	public class TermsFragment : MvxDialogFragment, View.IOnClickListener
	{
		private View _view;
		private Android.Support.V7.Widget.Toolbar _toolbar;

        /// <summary>
        /// Called when [create view].
        /// </summary>
        /// <param name="inflater">The inflater.</param>
        /// <param name="container">The container.</param>
        /// <param name="savedInstanceState">State of the saved instance.</param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			EnsureBindingContextSet(savedInstanceState);
			_view = this.BindingInflate(Resource.Layout.termsfragment, null);
			Dialog.SetCanceledOnTouchOutside(true);
			Dialog.SetTitle(LanguageResolver.TermsAndConditionsTitle);
			Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
			_toolbar = _view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_termsbar);
			_toolbar.Title = LanguageResolver.TermsAndConditionsTitle;
			_toolbar.NavigationIcon = Resources.GetDrawable(Resource.Drawable.ic_arrow_left);
			_toolbar.Click += (object sender, System.EventArgs e) => Dialog.Hide();

			TermsHtml();
			return _view;
		}

        /// <summary>
        /// Called when [resume].
        /// </summary>
        public override void OnResume()
		{
			base.OnResume();			
			// Auto size the dialog based on it's contents
			Dialog.Window.SetLayout(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent);

			// Make sure there is no background behind our view
			Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Black));

			// Disable standard dialog styling/frame/theme: our custom view should create full UI
			SetStyle((int)DialogFragmentStyle.NoTitle, Android.Resource.Style.ThemeTranslucentNoTitleBar);
		}

        /// <summary>
        /// Gets the terms view model.
        /// </summary>
        /// <value>
        /// The terms view model.
        /// </value>
        public TermsViewModel TermsViewModel
		{
			get { return (TermsViewModel)ViewModel; }
		}

        /// <summary>
        /// Called when [click].
        /// </summary>
        /// <param name="dialog">The dialog.</param>
        /// <param name="which">The which.</param>
        public void OnClick(IDialogInterface dialog, int which)
		{
		}

        #region IOnClickListener

        /// <summary>
        /// Called when [click].
        /// </summary>
        /// <param name="v">The v.</param>
        public void OnClick(View v)
		{
			InputMethodManager manager = (InputMethodManager)Activity.GetSystemService(Context.KeyguardService);
			manager.HideSoftInputFromWindow(v.WindowToken, 0);
		}

        #endregion

        /// <summary>
        /// Termses the HTML.
        /// </summary>
        void TermsHtml()
		{
			var help = _view.FindViewById<WebView>(Resource.Id.terms);
			help.LoadDataWithBaseURL("file:///android_asset/", TermsViewModel.TermsAndConditionsText, "text/html", "utf-8", "");

		}
	}
}
