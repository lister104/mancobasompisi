using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.ViewModels
{
    public class SentViewModel : MessengerBaseViewModel
    {
        #region Constructors

        public SentViewModel(IMvxMessenger messenger)
            : base(messenger)
        {

        }

        public void Init(DraftsViewModel item)
        {
            //there will be a ShowList() here
        }

        #endregion
    }
}
