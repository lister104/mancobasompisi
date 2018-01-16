using System.Collections;
using Android.Content;
using Android.Util;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Droid.Views;

namespace Mancoba.Sompisi.Droid.Classes.Controls
{
    public class MyMvxSpinner : MvxSpinner
    {
        public MyMvxSpinner(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        [MvxSetToNullAfterBinding]
        public new IEnumerable ItemsSource
        {
            get { return base.ItemsSource; }
            set
            {
                base.ItemsSource = null;
                base.ItemsSource = value;
            }
        }
    }
}