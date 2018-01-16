using Mancoba.Sompisi.Core.ViewModels;
using Mancoba.Sompisi.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;

namespace Mancoba.Sompisi.Core
{
	public class CustomApplicationStart : MvxNavigatingObject, IMvxAppStart
	{
		public void Start(object hint = null)
		{
			if (hint == null)
			{
				//ShowViewModel<NavigationViewModel> ();
				ShowViewModel<LoginViewModel>();
			}
			else
			{
				ShowViewModel<NavigationViewModel>();
			}
			//ShowViewModel<LoginViewModel>();
			//ShowViewModel<NavigationViewModel> ();
		}
	}
}

