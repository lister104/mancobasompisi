using System;
using System.Collections.Generic;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;

namespace Mancoba.Sompisi.Droid.Classes.Helpers
{
    public interface IFragmentHost
    {
        void Close(IMvxViewModel customPresenter);
        bool Show(MvxViewModelRequest request);
        Type ViewModelType { get; }
    }

    public interface ICustomPresenter
    {
        void Register(Type viewModelType, IFragmentHost host);
    }

    public class CustomPresenter : MvxAndroidViewPresenter, ICustomPresenter
    {
        private readonly Dictionary<Type, IFragmentHost> _dictionary = new Dictionary<Type, IFragmentHost>();

        public override void Show(MvxViewModelRequest request)
        {
            IFragmentHost host;

            if (_dictionary.TryGetValue(request.ViewModelType, out host))
            {
                //				if (request.ViewModelType == typeof(MapViewModel)) {
                //
                //					if (request.RequestedBy != null && request.RequestedBy == MvxRequestedByType.Other) {
                //						var naViewModel = _dictionary.Values.FirstOrDefault (v => v.ViewModelType == typeof(NavigationViewModel));
                //						naViewModel.Show (request);
                //					} else {
                //						var notherViewModel = _dictionary.Values.FirstOrDefault (v => v.ViewModelType == typeof(NavigationViewModel));
                //						naViewModel.Show (request);
                //					}
                //				}
                if (host.Show(request))
                {
                    return;
                }
            }
            base.Show(request);
        }

        public override void Close(IMvxViewModel viewModel)
        {
            //We implement this code to auto close fragments where the user makes a selection
            //This is actually stupid app behaviour
            IFragmentHost host = null;
            //var orgVm = viewModel as OrganisationsViewModel;
            //if (orgVm != null)
            //{
            //	if (_dictionary.TryGetValue(typeof(OrganisationsViewModel), out host))
            //	{
            //		host.Close(viewModel);
            //	}
            //}

            //if (host == null)
            //{
            //	var siteVm = viewModel as SitesViewModel;
            //	if (siteVm != null)
            //	{
            //		if (_dictionary.TryGetValue(typeof(SitesViewModel), out host))
            //		{
            //			host.Close(viewModel);
            //		}
            //	}
            //}

            //if (host == null)
            //{
            //	var siteVm = viewModel as ServersViewModel;
            //	if (siteVm != null)
            //	{
            //		if (_dictionary.TryGetValue(typeof(ServersViewModel), out host))
            //		{
            //			host.Close(viewModel);
            //		}
            //	}
            //}
            //if (host == null)
            //{
            //	var siteVm = viewModel as MeasurementTypesViewModel;
            //	if (siteVm != null)
            //	{
            //		if (_dictionary.TryGetValue(typeof(MeasurementTypesViewModel), out host))
            //		{
            //			host.Close(viewModel);
            //		}
            //	}
            //}

            //if (host == null)
            //{
            //	var siteVm = viewModel as MapRefreshRatesViewModel;
            //	if (siteVm != null)
            //	{
            //		if (_dictionary.TryGetValue(typeof(MapRefreshRatesViewModel), out host))
            //		{
            //			host.Close(viewModel);
            //		}
            //	}
            //}

            //if (host == null)
            //{
            //	var siteVm = viewModel as NotificationRuleViewModel;
            //	if (siteVm != null)
            //	{
            //		if (_dictionary.TryGetValue(typeof(NotificationRuleViewModel), out host))
            //		{
            //			host.Close(viewModel);
            //		}
            //	}
            //}

            if (host == null)
            {
                base.Close(viewModel);
            }
        }

        public void Register(Type viewModelType, IFragmentHost host)
        {
            _dictionary[viewModelType] = host;
        }
    }

    public enum MapPresenters
    {
        NavigationViewModel,
        FavouriteDetailsViewModel,
        AllFavouritesViewModel,
        FavouriteAssetsViewModel,
        FavouriteDriversViewModel
    }
}