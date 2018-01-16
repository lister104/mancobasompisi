using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Mancoba.Sompisi.Utils.Helpers
{
    public static class UtilsService
    {
        public const string FormatDate = "yyyy-MM-dd";
        public const string FormatDateTime = "yyyy-MM-dd HH:mm:ss";

        // ReSharper disable InconsistentNaming
        private static readonly Random _random = new Random();
        // ReSharper restore InconsistentNaming
        private static readonly object SyncLock = new object();

        /// <summary>
        /// Randoms the number.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public static int RandomNumber(int min, int max)
        {
            lock (SyncLock)
            {
                // synchronize
                return _random.Next(min, max);
            }
        }

        /// <summary>
        /// Merges the lists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mainList">The main list.</param>
        /// <param name="otherList">The other list.</param>
        /// <returns></returns>
        public static IList<T> MergeLists<T>(IList<T> mainList, IList<T> otherList)
        {
            if (mainList == null)
                mainList = new List<T>();

            if (otherList != null && otherList.Count > 0)
                return mainList.Union(otherList).ToList();

            return mainList;
        }

        #region GUID

        /// <summary>
        /// Generates the unique identifier.
        /// </summary>
        /// <returns></returns>
        public static Guid GenerateGuid()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Generates the identifier.
        /// </summary>
        /// <returns></returns>
        public static string GenerateId()
        {
            return GenerateGuid().ToString("D");
        }

        /// <summary>
        /// Generates the identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static string GenerateId(string id)
        {
            return string.IsNullOrWhiteSpace(id) ? GenerateGuid().ToString("D") : id;
        }

        #endregion

        #region StringExtension

        /// <summary>
        /// Determines whether [contains ignore case] [the specified original text].
        /// </summary>
        /// <param name="originalText">The original text.</param>
        /// <param name="compareText">The compare text.</param>
        /// <returns>
        ///   <c>true</c> if [contains ignore case] [the specified original text]; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsIgnoreCase(this string originalText, string compareText)
        {
            if (string.IsNullOrWhiteSpace(originalText) || string.IsNullOrWhiteSpace(compareText))
                return false;

            return
                (CultureInfo.CurrentCulture.CompareInfo.IndexOf(originalText, compareText, CompareOptions.IgnoreCase) >=
                 0);
        }

        /// <summary>
        /// Ares the equal ignore case.
        /// </summary>
        /// <param name="originalText">The original text.</param>
        /// <param name="compareText">The compare text.</param>
        /// <returns></returns>
        public static bool AreEqualIgnoreCase(this string originalText, string compareText)
        {
            return string.Equals(originalText, compareText, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// To the date string.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static string ToDateString(this DateTime date)
        {
            return date.ToString(FormatDate);
        }

        /// <summary>
        /// To the date time string.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static string ToDateTimeString(this DateTime date)
        {
            return date.ToString(FormatDateTime);
        }

        /// <summary>
        /// To the date.
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <returns></returns>
        public static DateTime ToDate(this string dateString)
        {
            return DateTime.ParseExact(dateString, FormatDate, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// To the date time.
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string dateString)
        {
            return DateTime.ParseExact(dateString, FormatDateTime, CultureInfo.InvariantCulture);
        }

        private const string Due = "Due";
        private const string Current = "Current";

        /// <summary>
        /// To the payment status.
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <param name="lastPaymentDate">The last payment date.</param>
        /// <returns></returns>
        public static string ToPaymentStatus(this string dateString, DateTime? lastPaymentDate)
        {
            if (lastPaymentDate == null)
                return Due;

            DateTime today = DateTime.Today;

            // past year
            if (lastPaymentDate.Value < today.AddYears(-1))
                return Current;

            return Due;
        }

        #endregion
    }
}