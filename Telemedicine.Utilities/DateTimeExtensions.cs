using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Utilities
{
    public static class DateTimeExtensions
    {
        public static string ToFormatTime(this DateTime dateTime)
        {
            return string.Format("{0: HH:mm}", dateTime);
        }
        public static string ToFormatDate(this DateTime dateTime)
        {
            return string.Format("{0: ddd, dd MMM yyy}", dateTime);
        }
        public static string ToFormatDateTime(this DateTime dateTime)
        {
            return string.Format("{0: ddd, dd MMM yyy HH:mm}", dateTime);
        }

        public static string ToTimezoneFormatTime(this DateTime dateTime, string timeZoneId)
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return string.Format("{0: HH:mm}", TimeZoneInfo.ConvertTimeFromUtc(dateTime, tzi));
        }

        public static string ToTimezoneFormatDate(this DateTime dateTime, string timeZoneId)
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return string.Format("{0: ddd, dd MMM yyy}", TimeZoneInfo.ConvertTimeFromUtc(dateTime, tzi));
        }

        public static string ToTimezoneFormatDateTime(this DateTime dateTime, string timeZoneId)
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return string.Format("{0: ddd, dd MMM yyy HH:mm}", TimeZoneInfo.ConvertTimeFromUtc(dateTime, tzi));
        }

        public static DateTime ToTimezoneFormatDateTimeToUTC(this DateTime dateTime, string timeZoneId)
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, tzi);
        }
    }
}
