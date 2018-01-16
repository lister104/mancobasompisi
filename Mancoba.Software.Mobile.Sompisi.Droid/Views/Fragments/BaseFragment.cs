using System;
using Android.Runtime;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Shared.Fragments;
using MvvmCross.Droid.Support.V4;

namespace Mancoba.Sompisi.Droid.Views.Fragments
{
	public abstract class BaseFragment<TViewModel> : MvxFragment, IMvxFragmentView<TViewModel> where TViewModel : class, IMvxViewModel
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFragment{TViewModel}"/> class.
        /// </summary>
        protected BaseFragment()
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFragment{TViewModel}"/> class.
        /// </summary>
        /// <param name="javaReference">The java reference.</param>
        /// <param name="transfer">The transfer.</param>
        protected BaseFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public new TViewModel ViewModel
		{
			get { return (TViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}
	}
}

