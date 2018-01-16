using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mancoba.Sompisi.Core.ViewModels;
using Mancoba.Sompisi.Core.ViewModels.Base;
using Mancoba.Sompisi.Droid.Classes.Adapters;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;
using Mancoba.Sompisi.Droid.Classes.Helpers;
using System.Threading.Tasks;

namespace Mancoba.Sompisi.Droid.Views
{
    [MvxFragment(typeof(NavigationViewModel), Resource.Id.content_frame)]
    [Register("mancoba.sompisi.droid.views.OutboxView")]
    public class OutboxView : MvxFragment
    {
        private MvxListView _listView;
        private DraftAdapter _adapter;
        private View _view;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            base.OnCreateView(inflater, container, savedInstanceState);

            _view = this.BindingInflate(Resource.Layout.draftitemview, null);

            var progress = _view.FindViewById<ProgressBar>(Resource.Id.progress);
            progress.IndeterminateDrawable.SetColorFilter(Colors.LightGray, PorterDuff.Mode.Multiply);

            var bindingSet = this.CreateBindingSet<OutboxView, OutboxViewModel>();
            bindingSet.Bind(this).For("HasFetched").To(vm => vm.HasFetched);

            bindingSet.Apply();

            return _view;
        }

        public override void OnResume()
        {
            base.OnResume();

            Task.Run(async () => { await ViewModel.GetOutboxDetails(); });
        }

        public void SetListViewHeightBasedOnChildren()
        {
            int desiredWidth = View.MeasureSpec.MakeMeasureSpec(_listView.Width, MeasureSpecMode.Unspecified);
            int totalHeight = 0;
            View view = null;
            for (int i = 0; i < _adapter.Count; i++)
            {
                view = _adapter.GetView(i, view, _listView);
                if (i == 0)
                    view.LayoutParameters = new ViewGroup.LayoutParams(desiredWidth, ViewGroup.LayoutParams.WrapContent);

                view.Measure(desiredWidth, (int)MeasureSpecMode.Unspecified);
                totalHeight += view.MeasuredHeight;
            }
            ViewGroup.LayoutParams p = _listView.LayoutParameters;
            p.Height = totalHeight + (_listView.DividerHeight * (_adapter.Count - 1));
            _listView.LayoutParameters = p;
        }

        #region Properties

        private OutboxViewModel _viewModel;

        public new OutboxViewModel ViewModel => _viewModel ?? (_viewModel = base.ViewModel as OutboxViewModel);

        private bool _hasFetched;
        public bool HasFetched
        {
            get { return _hasFetched; }
            set
            {
                _hasFetched = value;

                if (_hasFetched)
                {
                    SetListViewHeightBasedOnChildren();
                    _view.RequestLayout();
                }
            }
        }

        #endregion
    }
}