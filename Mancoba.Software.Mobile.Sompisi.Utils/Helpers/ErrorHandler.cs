using System;
using Mancoba.Sompisi.Utils.Helpers.UserInteraction;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace Mancoba.Sompisi.Utils.Helpers
{
	public static class ErrorHandler
	{
		public static Action<Exception> SendToRaygun { get; set; }

        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void HandleError(Exception exception)
		{
			HandleError(null, exception);
		}

        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void HandleError(string message, Exception exception)
		{
#if DEBUG
			try
			{
				MvxTrace.Trace((message ?? string.Empty) + (exception == null ? string.Empty : " " + exception.Message + "\r\n" + exception.StackTrace));
			}
			// ReSharper disable once EmptyGeneralCatchClause
			catch { }

			try
			{
				Mvx.Resolve<IUserInteraction>().Alert(message ?? string.Empty + (exception == null ? string.Empty : " " + exception.Message), null, "Error", LanguageResolver.Ok);
			}
			// ReSharper disable once EmptyGeneralCatchClause
			catch { }
#endif

			try
			{
				Mvx.Resolve<IUserInteraction>().Alert(message ?? string.Empty + (exception == null ? string.Empty : " " + exception.Message), null, "Error", LanguageResolver.Ok);
			}
			// ReSharper disable once EmptyGeneralCatchClause
			catch { }
		}
	}


	public static class RunningMode
	{

		public static bool IsRunning { get; set; }
		public static bool IsInForeground { get; set; }
	}
}

