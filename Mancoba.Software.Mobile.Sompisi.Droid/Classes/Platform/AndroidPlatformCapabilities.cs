using System;
using Android.App;
using Android.Net;
using Mancoba.Sompisi.Core.Messages;
using Mancoba.Sompisi.Utils.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Droid.Classes.Platform
{
    public class AndroidPlatformCapabilities : IPlatformCapabilities
    {
        bool _hasConnection;

        /// <summary>
        /// Determines whether [has network connection].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [has network connection]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasNetworkConnection()
        {
            try
            {
                ;
                if (Application.Context != null)
                {
                    var connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Android.Content.Context.ConnectivityService);
                    var activeConnection = connectivityManager.ActiveNetworkInfo;

                    if (activeConnection == null)
                    {
                        NotifyListeners(true);
                        return false;
                    }

                    NotifyListeners(true);
                    return activeConnection.IsConnected;
                }
            }
            catch (Exception)
            {
                return false;
            }
            NotifyListeners(false);
            return false;
        }

        /// <summary>
        /// Uses the local caching.
        /// </summary>
        /// <returns></returns>
        public bool UseLocalCaching()
        {
            return true;
        }

        /// <summary>
        /// Notifies the listeners.
        /// </summary>
        /// <param name="connectivity">if set to <c>true</c> [connectivity].</param>
        void NotifyListeners(bool connectivity)
        {
            if (connectivity == _hasConnection)
                return;

            _hasConnection = connectivity;
            var messanger = Mvx.Resolve<IMvxMessenger>();
            messanger.Publish(new ReachabilityMessage(_hasConnection));
        }

        public bool IsMapVisible { get; set; } = true;
    }
}