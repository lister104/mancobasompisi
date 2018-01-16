using Android.Runtime;
using Android.Views;
using Mancoba.Sompisi.Core.ViewModels;
using Mancoba.Sompisi.Core.ViewModels.Base;
using Mancoba.Sompisi.Droid.Views.Fragments;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;

namespace Mancoba.Sompisi.Droid.Views
{
	[MvxFragment(typeof(NavigationViewModel), Resource.Id.content_frame)]
	[Register("mancoba.sompisi.droid.views.SettingsView")]
	public class SettingsView : BaseFragment<SettingsViewModel> 
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			HasOptionsMenu = true;
			base.OnCreateView(inflater, container, savedInstanceState);

			var view = this.BindingInflate(Resource.Layout.settingsview, null);

			return view;
		}
	}
}

