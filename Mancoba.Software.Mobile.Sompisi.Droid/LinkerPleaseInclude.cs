using System.Collections.Specialized;
using System.Windows.Input;
using Android.App;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;

namespace Mancoba.Sompisi.Droid
{
    // This class is never actually executed, but when Xamarin linking is enabled it does how to ensure types and properties
    // are preserved in the deployed app
    public class LinkerPleaseInclude
    {
        /// <summary>
        /// Includes the specified button.
        /// </summary>
        /// <param name="button">The button.</param>
        public void Include(Button button)
        {
            button.Click += (s, e) => button.Text = button.Text + "";
        }

        /// <summary>
        /// Includes the specified check box.
        /// </summary>
        /// <param name="checkBox">The check box.</param>
        public void Include(CheckBox checkBox)
        {
            checkBox.CheckedChange += (sender, args) => checkBox.Checked = !checkBox.Checked;
        }

        /// <summary>
        /// Includes the specified switch.
        /// </summary>
        /// <param name="switch">The switch.</param>
        public void Include(Switch @switch)
        {
            @switch.CheckedChange += (sender, args) => @switch.Checked = !@switch.Checked;
        }

        /// <summary>
        /// Includes the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        public void Include(View view)
        {
            view.Click += (s, e) => view.ContentDescription = view.ContentDescription + "";
        }

        /// <summary>
        /// Includes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Include(TextView text)
        {
            text.TextChanged += (sender, args) => text.Text = "" + text.Text;
            text.Hint = "" + text.Hint;
        }

        /// <summary>
        /// Includes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Include(CheckedTextView text)
        {
            text.TextChanged += (sender, args) => text.Text = "" + text.Text;
            text.Hint = "" + text.Hint;
        }

        /// <summary>
        /// Includes the specified cb.
        /// </summary>
        /// <param name="cb">The cb.</param>
        public void Include(CompoundButton cb)
        {
            cb.CheckedChange += (sender, args) => cb.Checked = !cb.Checked;
        }

        /// <summary>
        /// Includes the specified sb.
        /// </summary>
        /// <param name="sb">The sb.</param>
        public void Include(SeekBar sb)
        {
            sb.ProgressChanged += (sender, args) => sb.Progress = sb.Progress + 1;
        }

        /// <summary>
        /// Includes the specified act.
        /// </summary>
        /// <param name="act">The act.</param>
        public void Include(Activity act)
        {
            act.Title = act.Title + "";
        }

        /// <summary>
        /// Includes the specified changed.
        /// </summary>
        /// <param name="changed">The changed.</param>
        public void Include(INotifyCollectionChanged changed)
        {
            changed.CollectionChanged += (s, e) => { var test = $"{e.Action}{e.NewItems}{e.NewStartingIndex}{e.OldItems}{e.OldStartingIndex}"; };
        }

        /// <summary>
        /// Includes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void Include(ICommand command)
        {
            command.CanExecuteChanged += (s, e) => { if (command.CanExecute(null)) command.Execute(null); };
        }

        /// <summary>
        /// Includes the specified injector.
        /// </summary>
        /// <param name="injector">The injector.</param>
        public void Include(MvvmCross.Platform.IoC.MvxPropertyInjector injector)
        {
            injector = new MvvmCross.Platform.IoC.MvxPropertyInjector();
        }

        /// <summary>
        /// Includes the specified changed.
        /// </summary>
        /// <param name="changed">The changed.</param>
        public void Include(System.ComponentModel.INotifyPropertyChanged changed)
        {
            changed.PropertyChanged += (sender, e) =>
            {
                var test = e.PropertyName;
            };
        }

        /// <summary>
        /// Includes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Include(MvxTaskBasedBindingContext context)
        {
            context.Dispose();
            var context2 = new MvxTaskBasedBindingContext();
            context2.Dispose();
        }
    }
}
