using Android.Content;
using Android.Support.V4.Widget;
using Android.Util;
using MvvmCross.Core.ViewModels;

namespace Mancoba.Sompisi.Droid.Classes.Controls
{
    public class MvxSwipeRefreshLayout : SwipeRefreshLayout
    {
        /// <summary>
        /// Gets or sets the refresh command.
        /// </summary>
        /// <value>The refresh command.</value>
        public IMvxAsyncCommand RefreshCommand { get; set; }

        public MvxSwipeRefreshLayout(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Init();
        }

        public MvxSwipeRefreshLayout(Context context)
            : base(context)
        {
            Init();
        }

        private void Init()
        {
            //This gets called when we pull down to refresh to trigger command
            Refresh += (sender, e) =>
            {
                var command = RefreshCommand;
                if (command == null)
                    return;
                command.Execute(null);
            };
        }
    }
}