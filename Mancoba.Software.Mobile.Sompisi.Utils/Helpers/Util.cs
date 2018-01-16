using System;
using System.Threading.Tasks;

namespace Mancoba.Sompisi.Utils.Helpers
{
	public static class Util
	{
		#region Settings

		public const string ServerIdentifier = "ServerIdentifier";
		public const string MeasurementIdentifier = "MeasurementIdentifier";
		public const string MapRefreshIdentifier = "MapRefreshIdentifier";
		public const string DebugInfoIdentifier = "DebugInfoIdentifier";

        #endregion

        #region Map

        /// <summary>
        /// Determines whether [is valid lat LNG] [the specified lat].
        /// </summary>
        /// <param name="lat">The lat.</param>
        /// <param name="lng">The LNG.</param>
        /// <returns>
        ///   <c>true</c> if [is valid lat LNG] [the specified lat]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidLatLng(double lat, double lng)
		{
			if (lat == -90 && lng == -180)
				return false;

			if (lat < -90 || lat > 90)
			{
				return false;

			}
			else if (lng < -180 || lng > 180)
			{
				return false;
			}
			return true;
		}

        #endregion

        /// <summary>
        /// Tries the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="maxAttempts">The maximum attempts.</param>
        /// <param name="failureMessage">The failure message.</param>
        /// <param name="onFailureAction">The on failure action.</param>
        public static void Try(Action action, byte maxAttempts = 2, string failureMessage = null, Action onFailureAction = null)
		{
			var attempts = 0;
			Exception lastException = null;
			while (attempts < maxAttempts)
			{
				try
				{
					action();
					break;
				}
				catch (Exception exception)
				{
					lastException = exception;
				}
				++attempts;
			}
			if (lastException != null)
				HandleFailure(lastException, failureMessage, onFailureAction);
		}

        /// <summary>
        /// Tries the specified action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action.</param>
        /// <param name="maxAttempts">The maximum attempts.</param>
        /// <param name="failureMessage">The failure message.</param>
        /// <param name="onFailureAction">The on failure action.</param>
        /// <param name="onFailureReturn">The on failure return.</param>
        /// <returns></returns>
        public static T Try<T>(Func<T> action, byte maxAttempts = 2, string failureMessage = null, Action onFailureAction = null, T onFailureReturn = default(T))
		{
			var result = default(T);
			var attempts = 0;
			Exception lastException = null;
			while (attempts < maxAttempts)
			{
				try
				{
					result = action();
					break;
				}
				catch (Exception exception)
				{
					lastException = exception;
				}
				++attempts;
			}
			if (lastException != null)
			{
				HandleFailure(lastException, failureMessage, onFailureAction);
				return onFailureReturn;
			}
			return result;
		}

        /// <summary>
        /// Tries the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action.</param>
        /// <param name="maxAttempts">The maximum attempts.</param>
        /// <param name="failureMessage">The failure message.</param>
        /// <param name="onFailureAction">The on failure action.</param>
        /// <param name="onFailureReturn">The on failure return.</param>
        /// <returns></returns>
        public static async Task<T> TryAsync<T>(Func<Task<T>> action, byte maxAttempts = 2, string failureMessage = null, Action onFailureAction = null, T onFailureReturn = default(T))
		{
			var result = default(T);
			var attempts = 0;
			Exception lastException = null;
			while (attempts < maxAttempts)
			{
				try
				{
					result = await action();
					break;
				}
				catch (TaskCanceledException)
				{
				//	HandleFailure(onFailureAction);
					return onFailureReturn;
				}
				catch (Exception exception)
				{
					lastException = exception;
				}
				++attempts;
			}
			if (lastException != null)
			{
				HandleFailure(lastException, failureMessage, onFailureAction);
				return onFailureReturn;
			}
			return result;
		}

        //public static string LastUpdated(string dateString)
        //{
        //	DateTime updateDateTime;
        //	if (!DateTime.TryParse(dateString, out updateDateTime))
        //		return dateString;

        //	var timeSpan = DateTime.UtcNow.Subtract(updateDateTime);
        //	if (timeSpan.Days > 1)
        //		return string.Format("({0} {1} {2} {3} {4} {5} {6})",
        //			timeSpan.Days,
        //			LanguageResolver.Days,
        //			timeSpan.Hours,
        //			LanguageResolver.Hours,
        //			timeSpan.Minutes,
        //			LanguageResolver.Minutes,
        //			LanguageResolver.Ago);

        //	if (timeSpan.Hours >= 1 || timeSpan.Days == 1)

        //	return string.Format("({0} {1} {2} {3} {4})", ((timeSpan.Days * 24) + timeSpan.Hours),
        //		LanguageResolver.Hours,
        //		timeSpan.Minutes,
        //		LanguageResolver.Minutes,
        //		LanguageResolver.Ago);

        //	return string.Format("({0} {1} {2} )", timeSpan.Minutes,
        //		LanguageResolver.Minutes,
        //		LanguageResolver.Ago);

        //}

        /// <summary>
        /// To the culture date string.
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <returns></returns>
        public static string ToCultureDateString(this string dateString)
		{
			DateTime dateTime;
			return DateTime.TryParse(dateString, out dateTime) ? String.Format("{0:ddd, dd MMM hh:mm tt}", DateTime.Parse(dateString)) : dateString;
		}

        /// <summary>
        /// Handles the failure.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="failureMessage">The failure message.</param>
        /// <param name="onFailureAction">The on failure action.</param>
        private static void HandleFailure(Exception exception, string failureMessage, Action onFailureAction)
		{
			ErrorHandler.HandleError(failureMessage, exception);
			HandleFailure(onFailureAction);
		}

        /// <summary>
        /// Handles the failure.
        /// </summary>
        /// <param name="onFailureAction">The on failure action.</param>
        private static void HandleFailure(Action onFailureAction)
		{
			if (onFailureAction == null)
				return;

			try
			{
				onFailureAction();
			}
			catch (Exception onFailureException)
			{
				ErrorHandler.HandleError("onFailureAction failed!", onFailureException);
			}
		}
	}
}



