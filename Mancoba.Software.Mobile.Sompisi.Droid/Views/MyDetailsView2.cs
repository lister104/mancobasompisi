using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Bluescore.DStv.Core.ViewModels;
using Bluescore.DStv.Core.ViewModels.Base;
using Bluescore.DStv.Droid.Classes.Helpers;
using Bluescore.DStv.Utils.Enums;
using Bluescore.DStv.Utils.Helpers;
using Bluescore.DStv.Utils.Helpers.UserInteraction;
using Bluescore.DStv.Utils.Language;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat.Widget;
using MvvmCross.Platform;

namespace Bluescore.DStv.Droid.Views
{
	
	[MvxFragment (typeof (NavigationViewModel), Resource.Id.content_frame)]
	[Register ("bluescore.tv.droid.views.MyDetailsView2")]
	public class MyDetailsView2 : MvxFragment
	{
		private AddressAddViewModel _viewModel;
		private IMenuItem _editItem;		
		private View _view;
		
		public MyDetailsView2()
		{
			RetainInstance = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			
			HasOptionsMenu = false;

			_view = this.BindingInflate(Resource.Layout.addressaddview, null);

			//Province			
            var provinceEditText = _view.FindViewById<MvxAppCompatSpinner>(Resource.Id.provinceEditText);
            provinceEditText.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(OnProviceSelectedChanged);

            var provinceLabel = _view.FindViewById<TextView>(Resource.Id.provinceLabel);
            var provinceUnderline = _view.FindViewById<View>(Resource.Id.provinceUnderline);
            provinceEditText.FocusChange += (sender, e) =>
            {
                if (!e.HasFocus)
                {
                    if (string.IsNullOrWhiteSpace(ViewModel.Province))
                    {
                        provinceLabel.SetTextColor(Colors.Red);
                        provinceUnderline.SetBackgroundColor(Colors.Red);
                        Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.CustAddNoProvinceError);
                    }
                    else
                    {
                        provinceLabel.SetTextColor(Colors.MediumGray);
                        provinceUnderline.SetBackgroundColor(Colors.MediumGray);
                    }
                }
            };


            //TOWN
            var townEditText = _view.FindViewById<MvxAppCompatSpinner>(Resource.Id.townEditText);
            townEditText.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(OnTownSelectedChanged);

            var townLabel = _view.FindViewById<TextView>(Resource.Id.townLabel);
            var townUnderline = _view.FindViewById<View>(Resource.Id.townUnderline);
            townEditText.FocusChange += (sender, e) =>
            {
                if (!e.HasFocus)
                {
                    if (string.IsNullOrWhiteSpace(ViewModel.Town))
                    {
                        townLabel.SetTextColor(Colors.Red);
                        townUnderline.SetBackgroundColor(Colors.Red);
                        Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.CustAddNoTownError);
                    }
                    else
                    {
                        townLabel.SetTextColor(Colors.MediumGray);
                        townUnderline.SetBackgroundColor(Colors.MediumGray);
                    }
                }
            };


            //SUBURB
            var suburbEditText = _view.FindViewById<MvxAppCompatSpinner>(Resource.Id.suburbEditText);
            suburbEditText.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(OnSuburbSelectedChanged);

            var suburbLabel = _view.FindViewById<TextView>(Resource.Id.suburbLabel);
            var suburbUnderline = _view.FindViewById<View>(Resource.Id.suburbUnderline);
            suburbEditText.FocusChange += (sender, e) =>
            {
                if (!e.HasFocus)
                {
                    if (string.IsNullOrWhiteSpace(ViewModel.Suburb))
                    {
                        suburbLabel.SetTextColor(Colors.Red);
                        suburbUnderline.SetBackgroundColor(Colors.Red);
                        Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.CustAddNoSuburbError);
                    }
                    else
                    {
                        suburbLabel.SetTextColor(Colors.MediumGray);
                        suburbUnderline.SetBackgroundColor(Colors.MediumGray);
                    }
                }
            };

            
            //Add
            var houseEditText = _view.FindViewById<EditText>(Resource.Id.houseEditText);
			var houseLabel = _view.FindViewById<TextView>(Resource.Id.houseLabel);
			var houseUnderline = _view.FindViewById<View>(Resource.Id.houseUnderline);
			houseEditText.FocusChange += (sender, e) => {
				if (!e.HasFocus)
				{
					if (string.IsNullOrEmpty(houseEditText.Text))
					{
						houseLabel.SetTextColor(Colors.Red);
						houseUnderline.SetBackgroundColor(Colors.Red);
						Mvx.Resolve<IUserInteraction>().ToastErrorAlert(LanguageResolver.CustAddNoHouseError);
					}
					else
					{
						houseLabel.SetTextColor(Colors.MediumGray);
						houseUnderline.SetBackgroundColor(Colors.MediumGray);
					}
				}
			};


			return _view;
		}

		public override void OnResume()
		{
			Resume();
		}

		public new AddressAddViewModel ViewModel
		{
			get { return _viewModel ?? (_viewModel = base.ViewModel as AddressAddViewModel); }
		}

		public NavigationFrom NavigationFrom { get; set; }

		protected void Resume()
		{
			base.OnResume();
		}


		#region Menu

	    private void OnProviceSelectedChanged(object sender, AdapterView.ItemSelectedEventArgs e)
	    {
	        Task.Run(async () =>
	        {
                var spinner = (MvxAppCompatSpinner) sender;
	            if (spinner.ItemsSource.Count() > 0)
	            {
                    var selected = (DropDownListItem) spinner.ItemsSource.ElementAt(e.Position);
	                if (selected != null)
	                {
	                    await ViewModel.UpdateTownsItemsSource(selected.Id);
	                    ViewModel.Province = selected.Id;

                        ViewModel.TownSelectedItem = ViewModel.TownItemsSource[0];
                        ViewModel.SuburbSelectedItem = ViewModel.SuburbItemsSource[0]; 
                        await ViewModel.UpdateHeading(1);
                    }
	            }
	        });
	    }

	    private void OnTownSelectedChanged(object sender, AdapterView.ItemSelectedEventArgs e)
	    {
	        Task.Run(async () =>
	        {
	            var spinner = (MvxAppCompatSpinner) sender;
	            if (spinner.ItemsSource.Count() > 0)
	            {
	                var selected = (DropDownListItem) spinner.ItemsSource.ElementAt(e.Position);
	                if (selected != null)
	                {
	                    await ViewModel.UpdateSuburbsItemsSource(selected.Id);
                        ViewModel.Town = selected.Id;
                        ViewModel.SuburbSelectedItem = ViewModel.SuburbItemsSource[0];
                        await ViewModel.UpdateHeading(2);
                    }
	            }
	        });
	    }

	    private void OnSuburbSelectedChanged(object sender, AdapterView.ItemSelectedEventArgs e)
	    {
	        Task.Run(async ()=>
	        {
	            var spinner = (MvxAppCompatSpinner) sender;
	            if (spinner.ItemsSource.Count() > 0)
	            {
	                var selected = (DropDownListItem) spinner.ItemsSource.ElementAt(e.Position);
	                if (selected != null)
	                {
                        await ViewModel.UpdateHeading(3);
                    }
	            }
	        });
	    }
		
		#endregion

	}
}

