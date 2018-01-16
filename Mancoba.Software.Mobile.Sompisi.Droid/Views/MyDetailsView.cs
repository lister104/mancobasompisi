using System.Threading.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Mancoba.Sompisi.Core.ViewModels;
using Mancoba.Sompisi.Core.ViewModels.Base;
using Mancoba.Sompisi.Utils.Enums;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;

namespace Mancoba.Sompisi.Droid.Views
{
	
	[MvxFragment (typeof (NavigationViewModel), Resource.Id.content_frame)]
	[Register ("mancoba.sompisi.droid.views.MyDetailsView")]
	public class MyDetailsView : MvxFragment
	{
		private MyDetailsViewModel _viewModel;		
		private View _view;
		
		public MyDetailsView()
		{
			RetainInstance = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			
			HasOptionsMenu = false;

			_view = this.BindingInflate(Resource.Layout.mydetailsview, null);

						
			return _view;
		}

		public override void OnResume()
		{
			base.OnResume();

			Task.Run(async () => { await ViewModel.LoadSystemUser(); });
		}

		public new MyDetailsViewModel ViewModel
		{
			get { return _viewModel ?? (_viewModel = base.ViewModel as MyDetailsViewModel); }
		}

		public NavigationFrom NavigationFrom { get; set; }

	}
}

