using System;
using System.Collections.Generic;
using Mancoba.Sompisi.Core.ViewModels;
using Mancoba.Sompisi.Droid.Views;
using Mancoba.Sompisi.Droid.Views.Fragments;
using Mancoba.Sompisi.Utils.Enums;
using MvvmCross.Droid.Shared.Caching;

namespace Mancoba.Sompisi.Droid.Classes.Caching
{
    internal class MainActivityFragmentCacheInfoFactory : MvxCachedFragmentInfoFactory
    {
        /// <summary>
        /// My fragments information
        /// </summary>
        private static readonly Dictionary<string, CustomFragmentInfo> MyFragmentsInfo = new Dictionary<string, CustomFragmentInfo>
        {
            //Provider
            //{
            //    typeof (ProviderListViewModel).ToString(),
            //    new CustomFragmentInfo(typeof (ProviderListViewModel).Name,
            //                           typeof (ProviderListView),
            //        typeof (ProviderListViewModel),    isRoot: true)
            //},
            //{
            //    typeof (ProviderDetailsViewModel).ToString(),
            //    new CustomFragmentInfo(typeof (ProviderDetailsViewModel).Name,
            //                           typeof (ProviderDetailsView),
            //        typeof (ProviderDetailsViewModel),    isRoot: false  ,addToBackstack : true)
            //},
            //{
            //    typeof (ProviderSearchViewModel).ToString(),
            //    new CustomFragmentInfo(typeof (ProviderSearchViewModel).Name,
            //                           typeof (ProviderSearchView),
            //        typeof (ProviderSearchViewModel),    isRoot: true)
            //},

            //Installer
            //{
            //    typeof (InstallerListViewModel).ToString(),
            //    new CustomFragmentInfo(typeof (InstallerListViewModel).Name,
            //                           typeof (InstallerListView),
            //        typeof (InstallerListViewModel),    isRoot: true)
            //},
            //{
            //    typeof (InstallerDetailsViewModel).ToString(),
            //    new CustomFragmentInfo(typeof (InstallerDetailsViewModel).Name,
            //                           typeof (InstallerDetailsView),
            //        typeof (InstallerDetailsViewModel),    isRoot: false  ,addToBackstack : true)
            //},
            //{
            //    typeof (InstallerSearchViewModel).ToString(),
            //    new CustomFragmentInfo(typeof (InstallerSearchViewModel).Name,
            //                           typeof (InstallerSearchView),
            //        typeof (InstallerSearchViewModel),    isRoot: true)
            //},
            //{
            //    typeof (MyFavouritesViewModel).ToString(),
            //    new CustomFragmentInfo(typeof (MyFavouritesViewModel).Name,
            //                           typeof (MyFavouritesView),
            //        typeof (MyFavouritesViewModel),    isRoot: true)
            //},
            //{
            //    typeof (MyBasketViewModel).ToString(),
            //    new CustomFragmentInfo(typeof (MyBasketViewModel).Name,
            //                           typeof (MyBasketView),
            //        typeof (MyBasketViewModel),    isRoot: true)
            //},
            //{
            //    typeof (MyPurchasesViewModel).ToString(),
            //    new CustomFragmentInfo(typeof (MyPurchasesViewModel).Name,
            //                           typeof (MyPurchasesView),
            //        typeof (MyPurchasesViewModel),    isRoot: true)
            //},
            //{
            //    typeof (MyWishListViewModel).ToString(),
            //    new CustomFragmentInfo(typeof (MyWishListViewModel).Name,
            //                           typeof (MyWishListView),
            //        typeof (MyWishListViewModel),    isRoot: true)
            //},
            //{
            //    typeof (MyDetailsViewModel).ToString(),
            //    new CustomFragmentInfo(typeof (MyDetailsViewModel).Name,
            //                           typeof (MyDetailsView),
            //        typeof (MyDetailsViewModel),    isRoot: true)
            //},
            //{
            //    typeof (SettingsViewModel).ToString(),
            //    new CustomFragmentInfo(typeof (SettingsViewModel).Name, typeof (SettingsView), typeof (SettingsViewModel),
            //         isRoot: true)
            //},
            //{
            //    typeof (TermsViewModel).ToString(),
            //    new CustomFragmentInfo(typeof (TermsViewModel).Name, typeof (TermsFragment), typeof (TermsViewModel),
            //         isRoot: true)
            //},

            //Applications
            {
                typeof (DraftsViewModel).ToString(),
                new CustomFragmentInfo(typeof (DraftsViewModel).Name,
                                       typeof (DraftView),
                    typeof (DraftsViewModel), isRoot: true)
            },
            {
                typeof (OutboxViewModel).ToString(),
                new CustomFragmentInfo(typeof (OutboxViewModel).Name,
                                       typeof (OutboxView),
                    typeof (OutboxViewModel), isRoot: true)
            },
            {
                typeof (SentListViewModel).ToString(),
                new CustomFragmentInfo(typeof (SentListViewModel).Name,
                                       typeof (SentListView),
                    typeof (SentListViewModel), isRoot: false, addToBackstack : true)
            },
            {
                typeof (SentSearchViewModel).ToString(),
                new CustomFragmentInfo(typeof (SentSearchViewModel).Name,
                                       typeof (SentSearchView),
                    typeof (SentSearchViewModel), isRoot: true)
            }
        };

        /// <summary>
        /// Gets the fragments registration data.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, CustomFragmentInfo> GetFragmentsRegistrationData()
        {
            return MyFragmentsInfo;
        }

        /// <summary>
        /// Creates the fragment information.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="fragmentType">Type of the fragment.</param>
        /// <param name="viewModelType">Type of the view model.</param>
        /// <param name="cacheFragment">if set to <c>true</c> [cache fragment].</param>
        /// <param name="addToBackstack">if set to <c>true</c> [add to backstack].</param>
        /// <returns></returns>
        public override IMvxCachedFragmentInfo CreateFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool cacheFragment = true, bool addToBackstack = false)
        {
            var viewModelTypeString = viewModelType.ToString();
            if (!MyFragmentsInfo.ContainsKey(viewModelTypeString))
                return base.CreateFragmentInfo(tag, fragmentType, viewModelType, cacheFragment, addToBackstack);

            var fragInfo = MyFragmentsInfo[viewModelTypeString];
            return fragInfo;
        }

        /// <summary>
        /// Gets the serializable fragment information.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns></returns>
        public override SerializableMvxCachedFragmentInfo GetSerializableFragmentInfo(IMvxCachedFragmentInfo objectToSerialize)
        {
            var baseSerializableCachedFragmentInfo = base.GetSerializableFragmentInfo(objectToSerialize);
            var customFragmentInfo = objectToSerialize as CustomFragmentInfo;

            return new SerializableCustomFragmentInfo(baseSerializableCachedFragmentInfo)
            {
                IsRoot = customFragmentInfo?.IsRoot ?? false
            };
        }

        /// <summary>
        /// Converts the serializable fragment information.
        /// </summary>
        /// <param name="fromSerializableMvxCachedFragmentInfo">From serializable MVX cached fragment information.</param>
        /// <returns></returns>
        public override IMvxCachedFragmentInfo ConvertSerializableFragmentInfo(SerializableMvxCachedFragmentInfo fromSerializableMvxCachedFragmentInfo)
        {
            var serializableCustomFragmentInfo = fromSerializableMvxCachedFragmentInfo as SerializableCustomFragmentInfo;
            var baseCachedFragmentInfo = base.ConvertSerializableFragmentInfo(fromSerializableMvxCachedFragmentInfo);

            return new CustomFragmentInfo(baseCachedFragmentInfo.Tag, baseCachedFragmentInfo.FragmentType,
                baseCachedFragmentInfo.ViewModelType, baseCachedFragmentInfo.AddToBackStack,
                serializableCustomFragmentInfo?.IsRoot ?? false)
            {
                ContentId = baseCachedFragmentInfo.ContentId,
                CachedFragment = baseCachedFragmentInfo.CachedFragment
            };
        }

        internal class SerializableCustomFragmentInfo : SerializableMvxCachedFragmentInfo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SerializableCustomFragmentInfo"/> class.
            /// </summary>
            public SerializableCustomFragmentInfo()
            {
                
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SerializableCustomFragmentInfo"/> class.
            /// </summary>
            /// <param name="baseFragmentInfo">The base fragment information.</param>
            public SerializableCustomFragmentInfo(SerializableMvxCachedFragmentInfo baseFragmentInfo)
            {
                AddToBackStack = baseFragmentInfo.AddToBackStack;
                ContentId = baseFragmentInfo.ContentId;
                FragmentType = baseFragmentInfo.FragmentType;
                Tag = baseFragmentInfo.Tag;
                ViewModelType = baseFragmentInfo.ViewModelType;
                CacheFragment = baseFragmentInfo.CacheFragment;
            }

            public bool IsRoot { get; set; }
        }
    }


	#region CustomFragmentInfo Class

	public class CustomFragmentInfo : MvxCachedFragmentInfo
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFragmentInfo"/> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="fragmentType">Type of the fragment.</param>
        /// <param name="viewModelType">Type of the view model.</param>
        /// <param name="cacheFragment">if set to <c>true</c> [cache fragment].</param>
        /// <param name="addToBackstack">if set to <c>true</c> [add to backstack].</param>
        /// <param name="isRoot">if set to <c>true</c> [is root].</param>
        /// <param name="navigationFrom">The navigation from.</param>
        public CustomFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool cacheFragment = true, bool addToBackstack = false, 	bool isRoot = false,NavigationFrom navigationFrom = NavigationFrom.NavMenu)
			: base(tag, fragmentType, viewModelType, cacheFragment, addToBackstack)
		{
			IsRoot = isRoot;
			NavigationFrom = navigationFrom;
		}

		public bool IsRoot { get; set; }

		public NavigationFrom NavigationFrom { get; set; }
	}

	#endregion
}