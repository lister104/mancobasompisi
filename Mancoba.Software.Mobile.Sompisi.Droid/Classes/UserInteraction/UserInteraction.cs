using System;
using Android.App;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform;
using System.Threading.Tasks;
using Mancoba.Sompisi.Utils.Enums;
using MvvmCross.Platform.Droid.Platform;
using Mancoba.Sompisi.Droid.Classes.Helpers;
using Mancoba.Sompisi.Utils.Helpers.UserInteraction;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace Mancoba.Sompisi.Droid.Classes.UserInteraction
{
    public class UserInteraction : IUserInteraction
    {
        /// <summary>
        /// Gets the current activity.
        /// </summary>
        /// <value>
        /// The current activity.
        /// </value>
        protected Activity CurrentActivity
        {
            get { return Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity; }
        }

        /// <summary>
        /// Confirms the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="okClicked">The ok clicked.</param>
        /// <param name="title">The title.</param>
        /// <param name="okButton">The ok button.</param>
        /// <param name="cancelButton">The cancel button.</param>
        public void Confirm(string message, Action okClicked, string title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
            Confirm(message, confirmed =>
            {
                if (confirmed)
                    okClicked();
            },
                title, okButton, cancelButton);
        }

        /// <summary>
        /// Confirms the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="answer">The answer.</param>
        /// <param name="title">The title.</param>
        /// <param name="okButton">The ok button.</param>
        /// <param name="cancelButton">The cancel button.</param>
        public void Confirm(string message, Action<bool> answer, string title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
            //Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction();
            Application.SynchronizationContext.Post(ignored =>
            {
                if (CurrentActivity == null) return;
                new AlertDialog.Builder(CurrentActivity)
                    .SetMessage(message)
                    .SetTitle(title)
                    .SetPositiveButton(okButton, delegate
                    {
                        if (answer != null)
                            answer(true);
                    })
                    .SetNegativeButton(cancelButton, delegate
                    {
                        if (answer != null)
                            answer(false);
                    })
                    .Show();
            }, null);
        }

        /// <summary>
        /// Confirms the asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="okButton">The ok button.</param>
        /// <param name="cancelButton">The cancel button.</param>
        /// <returns></returns>
        public Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel")
        {
            var tcs = new TaskCompletionSource<bool>();
            Confirm(message, tcs.SetResult, title, okButton, cancelButton);
            return tcs.Task;
        }

        /// <summary>
        /// Confirms the three buttons.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="answer">The answer.</param>
        /// <param name="title">The title.</param>
        /// <param name="positive">The positive.</param>
        /// <param name="negative">The negative.</param>
        /// <param name="neutral">The neutral.</param>
        public void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe")
        {
            Application.SynchronizationContext.Post(ignored =>
            {
                if (CurrentActivity == null) return;
                new AlertDialog.Builder(CurrentActivity)
                    .SetMessage(message)
                    .SetTitle(title)
                    .SetPositiveButton(positive, delegate
                    {
                        if (answer != null)
                            answer(ConfirmThreeButtonsResponse.Positive);
                    })
                    .SetNegativeButton(negative, delegate
                    {
                        if (answer != null)
                            answer(ConfirmThreeButtonsResponse.Negative);
                    })
                    .SetNeutralButton(neutral, delegate
                    {
                        if (answer != null)
                            answer(ConfirmThreeButtonsResponse.Neutral);
                    })
                    .Show();
            }, null);
        }

        /// <summary>
        /// Confirms the three buttons asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="positive">The positive.</param>
        /// <param name="negative">The negative.</param>
        /// <param name="neutral">The neutral.</param>
        /// <returns></returns>
        public Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe")
        {
            var tcs = new TaskCompletionSource<ConfirmThreeButtonsResponse>();
            ConfirmThreeButtons(message, tcs.SetResult, title, positive, negative, neutral);
            return tcs.Task;
        }

        /// <summary>
        /// Alerts the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="done">The done.</param>
        /// <param name="title">The title.</param>
        /// <param name="okButton">The ok button.</param>
        public void Alert(string message, Action done = null, string title = "", string okButton = "OK")
        {
            Application.SynchronizationContext.Post(ignored =>
            {
                if (CurrentActivity == null) return;
                new AlertDialog.Builder(CurrentActivity)
                    .SetMessage(message)
                    .SetTitle(title)
                    .SetPositiveButton(okButton, delegate
                    {
                        if (done != null)
                            done();
                    })
                    .Show();
            }, null);
        }

        /// <summary>
        /// Alerts the asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="okButton">The ok button.</param>
        /// <returns></returns>
        public Task AlertAsync(string message, string title = "", string okButton = "OK")
        {
            var tcs = new TaskCompletionSource<object>();
            Alert(message, () => tcs.SetResult(null), title, okButton);
            return tcs.Task;
        }

        /// <summary>
        /// Inputs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="okClicked">The ok clicked.</param>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="title">The title.</param>
        /// <param name="okButton">The ok button.</param>
        /// <param name="cancelButton">The cancel button.</param>
        /// <param name="initialText">The initial text.</param>
        public void Input(string message, Action<string> okClicked, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
        {
            Input(message, (ok, text) =>
            {
                if (ok)
                    okClicked(text);
            },
                placeholder, title, okButton, cancelButton, initialText);
        }

        /// <summary>
        /// Inputs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="answer">The answer.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="title">The title.</param>
        /// <param name="okButton">The ok button.</param>
        /// <param name="cancelButton">The cancel button.</param>
        /// <param name="initialText">The initial text.</param>
        public void Input(string message, Action<bool, string> answer, string hint = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
        {
            Application.SynchronizationContext.Post(ignored =>
            {
                if (CurrentActivity == null) return;
                var input = new EditText(CurrentActivity) { Hint = hint, Text = initialText };

                new AlertDialog.Builder(CurrentActivity)
                    .SetMessage(message)
                    .SetTitle(title)
                    .SetView(input)
                    .SetPositiveButton(okButton, delegate
                    {
                        if (answer != null)
                            answer(true, input.Text);
                    })
                    .SetNegativeButton(cancelButton, delegate
                    {
                        if (answer != null)
                            answer(false, input.Text);
                    })
                    .Show();
            }, null);
        }

        /// <summary>
        /// Inputs the asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="title">The title.</param>
        /// <param name="okButton">The ok button.</param>
        /// <param name="cancelButton">The cancel button.</param>
        /// <param name="initialText">The initial text.</param>
        /// <returns></returns>
        public Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
        {
            var tcs = new TaskCompletionSource<InputResponse>();
            Input(message, (ok, text) => tcs.SetResult(new InputResponse { Ok = ok, Text = text }), placeholder, title, okButton, cancelButton, initialText);
            return tcs.Task;
        }

        /// <summary>
        /// Toasts the alert.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="gravity">The gravity.</param>
        /// <param name="duration">The duration.</param>
        public void ToastAlert(string message, ToastGravity gravity = ToastGravity.Top, int duration = 3000)
        {
            Application.SynchronizationContext.Post(ignored =>
            {
                GravityFlags flag = gravity == ToastGravity.Top ? GravityFlags.Top : GravityFlags.Bottom;
                var toast = Toast.MakeText(Application.Context, message, ToastLength.Short);
                toast.SetGravity(flag | GravityFlags.Center, 0, 100);
                toast.Show();
            }, null);
        }

        /// <summary>
        /// Toasts the error alert.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="gravity">The gravity.</param>
        /// <param name="duration">The duration.</param>
        public void ToastErrorAlert(string message, ToastGravity gravity = ToastGravity.Top, int duration = 5000)
        {
            Application.SynchronizationContext.Post(ignored =>
            {
                GravityFlags flag = gravity == ToastGravity.Top ? GravityFlags.Top : GravityFlags.Bottom;
                var toast = Toast.MakeText(Application.Context, message, ToastLength.Short);
                toast.SetGravity(flag | GravityFlags.Center, 0, 100);
                toast.View.SetBackgroundColor(Colors.Red);
                toast.Show();
            }, null);
        }

        /// <summary>
        /// Toasts the error alert.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="gravity">The gravity.</param>
        /// <param name="duration">The duration.</param>
        public void ToastErrorAlert(Exception ex, ToastGravity gravity = ToastGravity.Top, int duration = 5000)
        {
            ToastErrorAlert(ex.InnerException?.Message ?? ex.Message, gravity, duration);
        }
    }
}