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

        public static int RandomNumber(int min, int max)
        {
            lock (SyncLock)
            {
                // synchronize
                return _random.Next(min, max);
            }
        }

        public static IList<T> MergeLists<T>(IList<T> mainList, IList<T> otherList)
        {
            if (mainList == null)
                mainList = new List<T>();

            if (otherList != null && otherList.Count > 0)
                return mainList.Union(otherList).ToList();

            return mainList;
        }

        #region GUID

        public static Guid GenerateGuid()
        {
            return Guid.NewGuid();
        }

        public static string GenerateId()
        {
            return GenerateGuid().ToString("D");
        }

        public static string GenerateId(string id)
        {
            return string.IsNullOrWhiteSpace(id) ? GenerateGuid().ToString("D") : id;
        }

        #endregion

        #region StringExtension

        public static bool ContainsIgnoreCase(this string originalText, string compareText)
        {
            if (string.IsNullOrWhiteSpace(originalText) || string.IsNullOrWhiteSpace(compareText))
                return false;

            return
                (CultureInfo.CurrentCulture.CompareInfo.IndexOf(originalText, compareText, CompareOptions.IgnoreCase) >=
                 0);
        }

        public static bool AreEqualIgnoreCase(this string originalText, string compareText)
        {
            return string.Equals(originalText, compareText, StringComparison.OrdinalIgnoreCase);
        }

        public static string ToDateString(this DateTime date)
        {
            return date.ToString(FormatDate);
        }

        public static string ToDateTimeString(this DateTime date)
        {
            return date.ToString(FormatDateTime);
        }

        public static DateTime ToDate(this string dateString)
        {
            return DateTime.ParseExact(dateString, FormatDate, CultureInfo.InvariantCulture);
        }

        public static DateTime ToDateTime(this string dateString)
        {
            return DateTime.ParseExact(dateString, FormatDateTime, CultureInfo.InvariantCulture);
        }

        private const string Due = "Due";
        private const string Current = "Current";

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