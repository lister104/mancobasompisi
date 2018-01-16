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

        /// <summary>
        /// Initializes a new instance of the <see cref="SentViewModel"/> class.
        /// </summary>
        /// <param name="messenger">The messenger.</param>
        public SentViewModel(IMvxMessenger messenger)
            : base(messenger)
        {

        }

        /// <summary>
        /// Initializes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Init(DraftsViewModel item)
        {
            //there will be a ShowList() here
        }

        #endregion
    }
}
