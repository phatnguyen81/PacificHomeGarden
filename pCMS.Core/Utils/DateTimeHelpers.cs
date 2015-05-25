using System;
using System.Globalization;

namespace pCMS.Core.Utils
{
    public class DateTimeHelpers
    {
        public static DateTime? ConvertStringToDateTime(string s)
        {
            DateTime dateTime;
            if(DateTime.TryParseExact(s, "dd/MM/yyyy",null, DateTimeStyles.None,out dateTime))
            {
                return dateTime;
            }
            return null;
        }

        public static DateTime Now
        {
            get
            {
                var userTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById(WorkContext.UserLoginInfo== null || String.IsNullOrWhiteSpace(WorkContext.UserLoginInfo.TimeZoneId)
                                                        ? TimeZone.CurrentTimeZone.StandardName
                                                        : WorkContext.UserLoginInfo.TimeZoneId);

                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimeZone);
            }
        }

        public static DateTime ConvertUtcToUserTimeZone(DateTime dt)
        {
            var userTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById(WorkContext.UserLoginInfo == null || String.IsNullOrWhiteSpace(WorkContext.UserLoginInfo.TimeZoneId)
                                                        ? TimeZone.CurrentTimeZone.StandardName
                                                        : WorkContext.UserLoginInfo.TimeZoneId);
            return TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.Utc, userTimeZone);
        }

        public static DateTime? ConvertUtcToUserTimeZone(DateTime? dt)
        {
            if (dt == null) return null;

            var userTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById(WorkContext.UserLoginInfo == null || String.IsNullOrWhiteSpace(WorkContext.UserLoginInfo.TimeZoneId)
                                                        ? TimeZone.CurrentTimeZone.StandardName
                                                        : WorkContext.UserLoginInfo.TimeZoneId);
            return TimeZoneInfo.ConvertTime(dt.Value, TimeZoneInfo.Utc, userTimeZone);
        }

        public static DateTime ConvertLocalToUserTimeZone(DateTime dt)
        {
            var userTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById(WorkContext.UserLoginInfo == null || String.IsNullOrWhiteSpace(WorkContext.UserLoginInfo.TimeZoneId)
                                                        ? TimeZone.CurrentTimeZone.StandardName
                                                        : WorkContext.UserLoginInfo.TimeZoneId);
            return TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.Local, userTimeZone);
        }

        public static DateTime? ConvertUserTimeZoneToUtc(DateTime? dt)
        {
            if (dt == null) return null;

            var userTimeZone = 
                TimeZoneInfo.FindSystemTimeZoneById(WorkContext.UserLoginInfo == null || String.IsNullOrWhiteSpace(WorkContext.UserLoginInfo.TimeZoneId)
                                                        ? TimeZone.CurrentTimeZone.StandardName
                                                        : WorkContext.UserLoginInfo.TimeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(dt.Value, userTimeZone);
        }

        public static DateTime ConvertUserTimeZoneToUtc(DateTime dt)
        {
            var userTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById(WorkContext.UserLoginInfo == null || String.IsNullOrWhiteSpace(WorkContext.UserLoginInfo.TimeZoneId)
                                                        ? TimeZone.CurrentTimeZone.StandardName
                                                        : WorkContext.UserLoginInfo.TimeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(dt, userTimeZone);
        }
    }
}