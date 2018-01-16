using Android.Content;
using Android.Views;
using Mancoba.Sompisi.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace Mancoba.Sompisi.Droid.Classes.Adapters
{
    #region Custom Adapter

    public class InstallAdapter : MvxAdapter
    {
        readonly Context _context;
        
        public InstallAdapter(Context context, IMvxAndroidBindingContext bindingContext)
            : base(context, bindingContext)
        {
            _context = context;

        }

        public override int GetItemViewType(int position)
        {
            if (position == 0)
                return 0;

            return 1;
        }

        public override int ViewTypeCount
        {
            get { return 2; }
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


            ProductItemViewModel installItem = dataContext as ProductItemViewModel;

            if (installItem.IsHeading)
            {
                templateId = Resource.Layout.installitemheading;
            }
            else
                templateId = Resource.Layout.installitem;


            View view = base.GetBindableView(convertView, dataContext, templateId);
            return view;
        }
    }

	#endregion
}

