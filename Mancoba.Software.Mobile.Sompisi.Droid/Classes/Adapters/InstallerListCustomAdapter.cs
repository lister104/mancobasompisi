using Android.Content;
using Android.Views;
using Android.Widget;
using Mancoba.Sompisi.Droid.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace Mancoba.Sompisi.Droid.Classes.Adapters
{
    #region Custom Adapter

    public class InstallerListCustomAdapter : MvxAdapter
    {
        private readonly Context _context;
        private readonly InstallerListView _view;
        private LinearLayout _backgroundStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerListCustomAdapter"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <param name="view">The view.</param>
        public InstallerListCustomAdapter(Context context, IMvxAndroidBindingContext bindingContext, InstallerListView view)
            : base(context, bindingContext)
        {
            _context = context;
            _view = view;
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
            get { return 1; }
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
            View view = base.GetBindableView(convertView, dataContext, templateId);
            var selectCheckboxImage = view.FindViewById<MvxImageView>(Resource.Id.selectCheckbox);
            selectCheckboxImage.Visibility = ViewStates.Invisible;
            return view;
        }
    }

    #endregion
}

