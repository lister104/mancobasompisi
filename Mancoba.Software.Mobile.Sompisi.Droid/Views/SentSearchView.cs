using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Mancoba.Sompisi.Core.ViewModels;
using Mancoba.Sompisi.Core.ViewModels.Base;
using Mancoba.Sompisi.Droid.Classes.Helpers;
using Mancoba.Sompisi.Utils.Enums;
using Mancoba.Sompisi.Utils.Helpers.UserInteraction;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platform;

namespace Mancoba.Sompisi.Droid.Views
{
    [MvxFragment(typeof(NavigationViewModel), Resource.Id.content_frame)]
    [Register("mancoba.sompisi.droid.views.SentSearchView")]
    public class SentSearchView : MvxFragment
    {
        private SentSearchViewModel _viewModel;
        private IMenuItem _editItem;
        private View _view;

        public SentSearchView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            return CreateView(Resource.Layout.sentsearchview);
        }

        public override void OnResume()
        {
            Resume();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return OptionsItemSelected(item);
        }

        public new SentSearchViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as SentSearchViewModel); }
        }

        public NavigationFrom NavigationFrom { get; set; }

        protected View CreateView(int layoutId)
        {
            HasOptionsMenu = false;

            _view = this.BindingInflate(layoutId, null);

            var searchTextEditText = _view.FindViewById<EditText>(Resource.Id.searchTextEditText);
            var searchTextLabel = _view.FindViewById<TextView>(Resource.Id.searchTextLabel);
            var searchTextUnderline = _view.FindViewById<View>(Resource.Id.searchTextUnderline);

            searchTextEditText.FocusChange += (sender, e) => {
                if (!e.HasFocus)
                {
                    if (string.IsNullOrEmpty(searchTextEditText.Text))
                    {
                        searchTextLabel.SetTextColor(Colors.Red);
                        searchTextUnderline.SetBackgroundColor(Colors.Red);
                        Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.SearchNoTextError);
                    }
                    else
                    {
                        searchTextLabel.SetTextColor(Colors.MediumGray);
                        searchTextUnderline.SetBackgroundColor(Colors.MediumGray);
                    }
                }
            };
            return _view;
        }

        protected void Resume()
        {
            base.OnResume();
        }

        #region Menu

        public void CreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            _editItem = menu.Add(Menu.None, Resource.Id.action_selections, Menu.First, "Edit");
            //_editItem = menu.Add(Menu.None, Resource.Id.action_selections1, Menu.None, "Assets");
            //	_editItem = menu.Add(Menu.None, Resource.Id.action_selections2, Menu.None, "Payment");

            _editItem.SetIcon(Resource.Drawable.menu);
            MenuItemCompat.SetShowAsAction(_editItem, MenuItemCompat.ShowAsActionAlways);
            _editItem.SetVisible(false);

            menu.RemoveItem(Resource.Id.action_selections);
        }

        public bool OptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.action_selections)
            {
                return true;
            }

            //if (item.ItemId == Resource.Id.action_selections1)
            //{

            //	return true;
            //}

            //if (item.ItemId == Resource.Id.action_selections2)
            //{

            //	return true;
            //}
            return false;
        }

        #endregion
    }
}