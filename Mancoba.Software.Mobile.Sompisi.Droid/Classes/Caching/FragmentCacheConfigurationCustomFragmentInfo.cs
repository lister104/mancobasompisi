

using MvvmCross.Droid.Shared.Caching;

namespace Mancoba.Sompisi.Droid.Classes.Caching
{
    class FragmentCacheConfigurationCustomFragmentInfo : FragmentCacheConfiguration<MainActivityFragmentCacheInfoFactory.SerializableCustomFragmentInfo>
    {
        private readonly MainActivityFragmentCacheInfoFactory _mainActivityFragmentCacheInfoFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FragmentCacheConfigurationCustomFragmentInfo"/> class.
        /// </summary>
        public FragmentCacheConfigurationCustomFragmentInfo()
        {
            _mainActivityFragmentCacheInfoFactory = new MainActivityFragmentCacheInfoFactory();
        }

        public override MvxCachedFragmentInfoFactory MvxCachedFragmentInfoFactory => _mainActivityFragmentCacheInfoFactory;
    }
}