namespace MedUA.Helpers
{
    using System;
    using System.Globalization;

    public static class DateTimeExtensions
    {
        public static string ToShowDate(this DateTime dateTime)
        {
            return dateTime.ToString("D", CultureInfo.CurrentCulture);
        }

        public static string ToShowDateTime(this DateTime dateTime)
        {
            return dateTime.ToString("g", CultureInfo.CurrentCulture);
        }

        public static string ToShowTime(this DateTime dateTime)
        {
            return dateTime.ToString("t", CultureInfo.CurrentCulture);
        }

        public static string ToCompareTime(this DateTime dateTime)
        {
            var result = new DateTime(1900, 1, 1, dateTime.Hour, dateTime.Minute, dateTime.Second);
            return result.ToString("O");
        }

        public static string ToCompareDate(this DateTime dateTime)
        {
            var result = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
            return result.ToString("O");
        }
        
    }
}