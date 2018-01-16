using Android.Content;
using Android.Views;
using Android.Widget;
using Mancoba.Sompisi.Droid.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace Mancoba.Sompisi.Droid.Classes.Adapters
{

    #region Custom Adapter

    public class ProviderListCustomAdapter : MvxAdapter
    {
        private readonly Context _context;
        private readonly ProviderListView _view;
        private LinearLayout _backgroundStatus;

        public ProviderListCustomAdapter(Context context, IMvxAndroidBindingContext bindingContext,
            ProviderListView view)
            : base(context, bindingContext)
        {
            _context = context;
            _view = view;
        }

        public override int GetItemViewType(int position)
        {
            return 1;
        }

        public override int ViewTypeCount
        {
            get { return 1; }
        }

        protected override IMvxListItemView CreateBindableView(object dataContext, int templateId)
        {
            var view = base.CreateBindableView(dataContext, templateId);
            return view;
        }

        protected override View GetBindableView(View convertView, object dataContext)
        {
            return base.GetBindableView(convertView, dataContext);
        }

        protected override View GetBindableView(View convertView, object dataContext, int templateId)
        {
            View view = base.GetBindableView(convertView, dataContext, templateId);
            var selectCheckboxImage = view.FindViewById<MvxImageView>(Resource.Id.selectCheckbox);
            selectCheckboxImage.Visibility = ViewStates.Invisible;
            return view;
        }


    }

    #endregion
}

