using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.ViewModels
{
    public class OutboxViewModel : MessengerBaseViewModel
    {
        private bool _isOutboxViewLoaded;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OutboxViewModel" /> class.
        /// </summary>
        /// <param name="messenger">The messenger.</param>
        public OutboxViewModel(IMvxMessenger messenger)
                    : base(messenger)
        {

        }

        /// <summary>
        /// Initializes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Init(DraftsViewModel item)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the outbox details.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Checks the loading status.
        /// </summary>
        private void CheckLoadingStatus()
        {
            IsLoading = true;
            if (_isOutboxViewLoaded)
                IsLoading = false;
        }

        #endregion
    }
}
