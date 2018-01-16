using System;
using Android.Runtime;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Shared.Fragments;
using MvvmCross.Droid.Support.V4;

namespace Mancoba.Sompisi.Droid.Views.Fragments
{
	public abstract class BaseFragment<TViewModel> : MvxFragment, IMvxFragmentView<TViewModel> where TViewModel : class, IMvxViewModel
	{
		protected BaseFragment()
		{
		}

		protected BaseFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public new TViewModel ViewModel
		{
			get { return (TViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}
	}
}

