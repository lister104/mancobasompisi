using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.ViewModels
{
    public class OutboxViewModel : MessengerBaseViewModel
    {
        private bool _isOutboxViewLoaded;

        #region Constructors

        public OutboxViewModel(IMvxMessenger messenger)
            : base(messenger)
        {

        }

        public void Init(DraftsViewModel item)
        {

        }

        #endregion

        #region Public Methods

        public async Task GetOutboxDetails()
        {
            _isOutboxViewLoaded = false;
            IsLoading = true;

            //var model = await Mvx.Resolve<IMobileDataService>().GetDraft(Id);
            //Id = model.Id;
            //For now
            await Task.FromResult(true);

            _isOutboxViewLoaded = true;
            CheckLoadingStatus();
        }

        private void CheckLoadingStatus()
        {
            IsLoading = true;
            if (_isOutboxViewLoaded)
                IsLoading = false;
        }

        #endregion
    }
}
