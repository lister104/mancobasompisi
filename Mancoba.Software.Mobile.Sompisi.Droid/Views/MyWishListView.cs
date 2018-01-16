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
	[MvxFragment(typeof(NavigationViewModel), Resource.Id.content_frame)]
	[Register("mancoba.sompisi.droid.views.MyWishListView")]
    public class MyWishListView : MvxFragment
    {
        private MyWishListViewModel _viewModel;
        private View _view;

        public MyWishListView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            HasOptionsMenu = false;
            base.OnCreateView(inflater, container, savedInstanceState);

            _view = this.BindingInflate(Resource.Layout.mywishlistview, null);

            return _view;
        }

        public override void OnResume()
        {
            Resume();
        }

        public new MyWishListViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as MyWishListViewModel); }
        }

        public NavigationFrom NavigationFrom { get; set; }

        protected void Resume()
        {
            base.OnResume();
        }
    }
}

