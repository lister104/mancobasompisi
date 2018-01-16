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

        /// <summary>
        /// Initializes a new instance of the <see cref="SentSearchView"/> class.
        /// </summary>
        public SentSearchView()
        {
            RetainInstance = true;
        }

        /// <summary>
        /// Called when [create view].
        /// </summary>
        /// <param name="inflater">The inflater.</param>
        /// <param name="container">The container.</param>
        /// <param name="savedInstanceState">State of the saved instance.</param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            return CreateView(Resource.Layout.sentsearchview);
        }

        /// <summary>
        /// Called when [resume].
        /// </summary>
        public override void OnResume()
        {
            Resume();
        }

        /// <summary>
        /// Called when [options item selected].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return OptionsItemSelected(item);
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public new SentSearchViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as SentSearchViewModel); }
        }

        public NavigationFrom NavigationFrom { get; set; }

        /// <summary>
        /// Creates the view.
        /// </summary>
        /// <param name="layoutId">The layout identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Resumes this instance.
        /// </summary>
        protected void Resume()
        {
            base.OnResume();
        }

        #region Menu

        /// <summary>
        /// Creates the options menu.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="inflater">The inflater.</param>
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

        /// <summary>
        /// Optionses the item selected.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
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