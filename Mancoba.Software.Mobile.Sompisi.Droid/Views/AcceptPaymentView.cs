using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Views.Animations;
using Android.Views.InputMethods;
using Android.Widget;
using Bluescore.DStv.Core.ViewModels;
using Bluescore.DStv.Core.ViewModels.Base;
using Bluescore.DStv.Droid.Classes.Adapters;
using Bluescore.DStv.Droid.Classes.Helpers;
using Bluescore.DStv.Utils.Enums;
using Bluescore.DStv.Utils.Language;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;
using MvxSwipeRefreshLayout = Bluescore.DStv.Droid.Classes.Controls.MvxSwipeRefreshLayout;
namespace Bluescore.DStv.Droid.Views
{
	[MvxFragment (typeof (NavigationViewModel), Resource.Id.content_frame)]
	[Register ("bluescore.tv.droid.views.AcceptPaymentView")]
	public class AcceptPaymentView : MvxFragment, View.IOnClickListener
	{

		public AcceptPaymentView ()
		{
			RetainInstance = true;

		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);

			return CreateView (Resource.Layout.customerlistview);

		}

		public override void OnResume ()
		{

			Resume ();
			 
		}


		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{

			CreateOptionsMenu (menu, inflater);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{

			return OptionsItemSelected (item);
		}


		#region Properties

		private PaymentViewModel viewModel;
		public new PaymentViewModel ViewModel {
			get { return viewModel ?? (viewModel = base.ViewModel as PaymentViewModel); }
		}


		#endregion

		private IMenuItem _editCancelItem;
		private TextView _cancelTexView;
		private EditText _searchText;
		TextView _countText;
		private MvxListView _listview;
		private float _scrollDistance;
		private bool _isScrollingDown;
		private bool _searchBarIsVisible;
		private View _view;
		WorkOrdersCustomAdapter _adapter;
		Button _btn;


		public NavigationFrom NavigationFrom { get; set; }

		protected View CreateView(int layoutId)
		{
			HasOptionsMenu = true;

			_view = this.BindingInflate(layoutId, null);

			_listview = _view.FindViewById<MvxListView>(Resource.Id.orderlist);
			_adapter = new WorkOrdersCustomAdapter(this.Activity, (IMvxAndroidBindingContext)BindingContext, this);
			_listview.Adapter = _adapter;
			_listview.ScrollStateChanged += HandleScrollStateChanged;
			_listview.Scroll += HandleScroll;

			_searchText = _view.FindViewById<EditText>(Resource.Id.search);
			_countText = _view.FindViewById<TextView>(Resource.Id.count);
			_btn = _view.FindViewById<Button>(Resource.Id.button);

			var progress = _view.FindViewById<ProgressBar>(Resource.Id.progress);
			progress.IndeterminateDrawable.SetColorFilter(Colors.LightGray, PorterDuff.Mode.Multiply);

			var refresher = _view.FindViewById<MvxSwipeRefreshLayout>(Resource.Id.refresher);
			refresher.SetColorSchemeResources(Resource.Color.green,
				Resource.Color.light,
				Resource.Color.white,
				Resource.Color.light_grey);
			refresher.RefreshCommand = ViewModel.RefreshCommand;

			var bindingSet = this.CreateBindingSet<AcceptPaymentView, PaymentViewModel>();
			bindingSet.Bind(this).For("NavigationFrom").To(vm => vm.NavigationFrom);
			bindingSet.Bind(this).For("EditMode").To(vm => vm.EditMode);
			bindingSet.Apply();

			return _view;
		}

		protected void Resume()
		{
			base.OnResume();
			InputMethodManager manager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
			manager.HideSoftInputFromWindow(_view.WindowToken, 0);


			Task.Run(async () => {
				await ViewModel.FetchWorkOrders();
				ViewModel.SwitchBack();
			});

		}

		public void CreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			//_editItem = menu.Add (Menu.None, Resource.Id.action_selections, Menu.None, "Edit");
			//_editItem.SetIcon (EditMode ? Resource.Drawable.ic_selections_grey : Resource.Drawable.ic_selections_green);
			//MenuItemCompat.SetShowAsAction (_editItem, MenuItemCompat.ShowAsActionAlways);
			_editCancelItem = menu.Add(Menu.None, Resource.Id.menu_cancel, Menu.None, "Edit");
			MenuItemCompat.SetShowAsAction(_editCancelItem, MenuItemCompat.ShowAsActionAlways);
			//_editCancelItem.SetTitle(EditMode ? LanguageResolver.Cancel : LanguageResolver.Edit);
			_editCancelItem.SetActionView(Resource.Layout.action_menuitem);
			_cancelTexView = _editCancelItem.ActionView.FindViewById<TextView>(Resource.Id.cancelitem);
			_cancelTexView.SetOnClickListener(this);
			_cancelTexView.Text = EditMode ? LanguageResolver.Cancel : LanguageResolver.Edit;

			//_editItem.SetVisible (false);
			if (ViewModel.IsShowingMyOrders)
				return;

			menu.RemoveItem(Resource.Id.action_selections);
			menu.RemoveItem(Resource.Id.menu_cancel);
		}


		public bool OptionsItemSelected(IMenuItem item)
		{

			if (item.ItemId == Resource.Id.menu_cancel)
			{

				ViewModel.SwitchEditMode();
				return true;
			}
			return false;
		}


		#region Private Methods
		private void ShowSearchBar()
		{
			_searchBarIsVisible = true;

			var anim = new TranslateAnimation(_listview.GetX(), _listview.GetX(), _listview.GetY(), 0)
			{
				Duration = 50,
				FillAfter = true
			};

			_listview.StartAnimation(anim);

			_searchText.Visibility = ViewStates.Visible;
			_countText.Visibility = ViewStates.Visible;

			var searchAnim = new TranslateAnimation(_searchText.GetX(), _searchText.GetX(), _searchText.GetY(), 0)
			{
				Duration = 50,
				FillBefore = true
			};

			_searchText.StartAnimation(searchAnim);

			var countAnim = new TranslateAnimation(_countText.GetX(), _countText.GetX(), _countText.GetY(), 0)
			{
				Duration = 50,
				FillBefore = true
			};

			_countText.StartAnimation(countAnim);
		}

		private void HideSearchBar()
		{
			_searchBarIsVisible = false;

			var searchAnim = new TranslateAnimation(_searchText.GetX(), _searchText.GetX(), _searchText.GetY(), -_scrollDistance)
			{
				Duration = 100,
				FillAfter = true
			};

			var countAnim = new TranslateAnimation(_countText.GetX(), _countText.GetX(), _countText.GetY(), -_countText.Height)
			{
				Duration = 100,
				FillAfter = true
			};

			_searchText.StartAnimation(searchAnim);
			_searchText.Visibility = ViewStates.Gone;

			_countText.StartAnimation(countAnim);
			_countText.Visibility = ViewStates.Gone;

			var anim = new TranslateAnimation(_listview.GetX(), _listview.GetX(), _listview.GetY(), _searchText.GetY())
			{
				Duration = 50,
				FillAfter = true
			};

			_listview.StartAnimation(anim);
		}

		void HandleScroll(object sender, AbsListView.ScrollEventArgs e)
		{
			//if (_scrollDistance <= 1) {
			//	_searchBarIsVisible = true;        // HandleScroll gets called when the view is created, which causes search bar to be removed
			//	_isScrollingDown = true;
			//	_scrollDistance = _searchText.Height;
			//}

			//if (_isScrollingDown) {
			//	if (!_searchBarIsVisible) {
			//		ShowSearchBar ();
			//	}
			//} else if (!_isScrollingDown) {
			//	if (_searchBarIsVisible) {
			//		HideSearchBar ();
			//	}
			//}
		}

		int _lastTopVisibleItem;

		void HandleScrollStateChanged(object sender, AbsListView.ScrollStateChangedEventArgs e)
		{
			//if (e.View.Id == _listview.Id) {
			//	switch (e.ScrollState) {
			//	case ScrollState.Fling:
			//	case ScrollState.TouchScroll:
			//		// Scrolled down

			//		int currentTopVisibleItem = _listview.FirstVisiblePosition;

			//		if (currentTopVisibleItem > _lastTopVisibleItem) {
			//			_isScrollingDown = false;

			//			if (_searchBarIsVisible) {
			//				HideSearchBar ();
			//			}
			//		} else if (currentTopVisibleItem < _lastTopVisibleItem) {
			//			_isScrollingDown = true;

			//			if (!_searchBarIsVisible) {
			//				ShowSearchBar ();
			//			}
			//		}

			//		_lastTopVisibleItem = currentTopVisibleItem;

			//		break;
			//	}
			//}
		}

		public void OnClick(View v)
		{
			OnOptionsItemSelected(_editCancelItem);
			_cancelTexView.Text = EditMode ? LanguageResolver.Cancel : LanguageResolver.Edit;
		}

		#endregion



		#region Properties

		private bool _editMode;
		public bool EditMode
		{
			get { return _editMode; }
			set
			{
				if (_editMode != value)
				{
					_editMode = value;
					_adapter.NotifyDataSetChanged();
				}
				else
					_editMode = value;


				if (_editCancelItem == null)
					return;

				//_editCancelItem.SetTitle(value ? LanguageResolver.Cancel : LanguageResolver.Edit);
				//MenuItemCompat.SetShowAsAction (_editCancelItem, MenuItemCompat.ShowAsActionAlways);

				_cancelTexView.Text = EditMode ? LanguageResolver.Cancel : LanguageResolver.Edit;


			}

		}




		#endregion
	}
}

