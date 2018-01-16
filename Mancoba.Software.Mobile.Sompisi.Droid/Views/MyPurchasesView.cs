using Android.Runtime;
using Android.Views;
using Mancoba.Sompisi.Core.ViewModels;
using Mancoba.Sompisi.Core.ViewModels.Base;
using Mancoba.Sompisi.Droid.Views.Fragments;
using Mancoba.Sompisi.Utils.Enums;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;

namespace Mancoba.Sompisi.Droid.Views
{
	[MvxFragment(typeof(NavigationViewModel), Resource.Id.content_frame)]
	[Register("mancoba.sompisi.droid.views.MyPurchasesView")]
    public class MyPurchasesView : MvxFragment
    {
        private MyPurchasesViewModel _viewModel;
        private View _view;

        public MyPurchasesView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            HasOptionsMenu = false;
            base.OnCreateView(inflater, container, savedInstanceState);

            _view = this.BindingInflate(Resource.Layout.mypurchasesview, null);

            return _view;
        }

        public override void OnResume()
        {
            Resume();
        }

        public new MyPurchasesViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as MyPurchasesViewModel); }
        }

        public NavigationFrom NavigationFrom { get; set; }

        protected void Resume()
        {
            base.OnResume();
        }
    }
}

