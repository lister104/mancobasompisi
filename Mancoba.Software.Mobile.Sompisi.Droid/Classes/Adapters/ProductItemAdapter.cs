using Android.Content;
using Android.Views;
using Mancoba.Sompisi.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace Mancoba.Sompisi.Droid.Classes.Adapters
{
    #region Custom Adapter

    public class ProductItemAdapter : MvxAdapter
    {
        private readonly Context _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductItemAdapter"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="bindingContext">The binding context.</param>
        public ProductItemAdapter(Context context, IMvxAndroidBindingContext bindingContext)
            : base(context, bindingContext)
        {
            _context = context;

        }

        /// <summary>
        /// Get the type of View that will be created by <c><see cref="!:Android.Widget.Adapter.getView(int, android.view.View, android.view.ViewGroup)" /></c> for the specified item.
        /// </summary>
        /// <param name="position">The position of the item within the adapter's data set whose view type we
        /// want.</param>
        /// <returns>
        /// To be added.
        /// </returns>
        /// <remarks>
        /// <para tool="javadoc-to-mdoc">Get the type of View that will be created by <c><see cref="!:Android.Widget.Adapter.getView(int, android.view.View, android.view.ViewGroup)" /></c> for the specified item.</para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/widget/BaseAdapter.html#getItemViewType(int)" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para>
        /// </remarks>
        /// <since version="Added in API level 1" />
        public override int GetItemViewType(int position)
        {
            if (position == 0)
                return 0;

            return 1;
        }

        /// <summary>
        /// </summary>
        /// <value>
        /// To be added.
        /// </value>
        /// <remarks>
        /// <para tool="javadoc-to-mdoc" />
        /// <para tool="javadoc-to-mdoc">
        /// Returns the number of types of Views that will be created by
        /// <c><see cref="!:Android.Widget.Adapter.getView(int, android.view.View, android.view.ViewGroup)" /></c>. Each type represents a set of views that can be
        /// converted in <c><see cref="!:Android.Widget.Adapter.getView(int, android.view.View, android.view.ViewGroup)" /></c>. If the adapter always returns the same
        /// type of View for all items, this method should return 1.
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        /// This method will only be called when when the adapter is set on the
        /// the <c><see cref="T:Android.Widget.AdapterView" /></c>.
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/widget/BaseAdapter.html#getViewTypeCount()" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para>
        /// </remarks>
        /// <since version="Added in API level 1" />
        public override int ViewTypeCount
        {
            get { return 2; }
        }

        /// <summary>
        /// Creates the bindable view.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <param name="templateId">The template identifier.</param>
        /// <returns></returns>
        protected override IMvxListItemView CreateBindableView(object dataContext, int templateId)
        {
            var view = base.CreateBindableView(dataContext, templateId);
            return view;
        }

        /// <summary>
        /// Gets the bindable view.
        /// </summary>
        /// <param name="convertView">The convert view.</param>
        /// <param name="dataContext">The data context.</param>
        /// <returns></returns>
        protected override View GetBindableView(View convertView, object dataContext)
        {
            return base.GetBindableView(convertView, dataContext);
        }

        /// <summary>
        /// Gets the bindable view.
        /// </summary>
        /// <param name="convertView">The convert view.</param>
        /// <param name="dataContext">The data context.</param>
        /// <param name="templateId">The template identifier.</param>
        /// <returns></returns>
        protected override View GetBindableView(View convertView, object dataContext, int templateId)
        {
            ProductItemViewModel assetItem = dataContext as ProductItemViewModel;

            if (assetItem.IsHeading)
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

