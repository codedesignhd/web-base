using System;
using System.Globalization;

namespace CodeDesign.Utilities
{
    public abstract class DateTimeUtils
    {
        /// <summary>
        /// Convert DateTime to UnixTime
        /// </summary>
        public static long TimeInEpoch(DateTime? dt = null)
        {
            if (!dt.HasValue)
            {
                dt = DateTime.UtcNow;
            }
            DateTimeOffset dt_off = DateTime
                .SpecifyKind(dt.Value, DateTimeKind.Local);
            return dt_off.ToUnixTimeSeconds();
        }

        /// <summary>
        /// Convert UnixTime to DateTime
        /// </summary>
        public static DateTime EpochToTime(long epoch)
        {
            return new DateTime(1970, 1, 1)
                .AddSeconds(epoch)
                .ToLocalTime();
        }



        /// <summary>
        /// Convert Epoch to time string
        /// </summary>
        public static string EpochToTimeString(long epoch, string format = "dd/MM/yyyy HH:mm")
        {
            DateTime dt = EpochToTime(epoch);
            try
            {
                return dt.ToString(format);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// return format hh:MM:ss present by seconds
        /// </summary>
        public static string OffsetBy(double seconds, bool isAddWord = false)
        {
            TimeSpan ts = TimeSpan.FromSeconds(seconds);
            string mins = ts.Minutes < 10 ? string.Format("0{0}", ts.Minutes) : Convert.ToString(ts.Minutes);
            string secs = ts.Seconds < 10 ? string.Format("0{0}", ts.Seconds) : Convert.ToString(ts.Seconds);
            if (ts.Hours > 0)
            {
                string hours = ts.Hours < 10 ? string.Format("0{0}", ts.Hours) : Convert.ToString(ts.Hours);
                if (isAddWord)
                {
                    return string.Format("{2} giờ {0} phút {1} giây", hours, mins, secs);
                }
                else
                {
                    return string.Format("{0}:{1}", mins, secs);
                }
            }
            else
            {
                if (isAddWord)
                {
                    return string.Format("{0} phút {1} giây", mins, secs);
                }
                else
                {
                    return string.Format("{0}:{1}", mins, secs);
                }
            }

        }

        public static long StringToEpoch(string date)
        {
            if (!string.IsNullOrWhiteSpace(date))
            {
                if (DateTime.TryParse(date, out DateTime dt))
                {
                    return TimeInEpoch(dt);
                }
            }
            return TimeInEpoch();
        }

        public static bool IsValidDate(string date)
        {
            return DateTime.TryParse(date, new CultureInfo("vi-VN"), DateTimeStyles.None, out _);
        }



    }
    public static class DateTimeExtensions
    {
        public static string ToLocalTime(this DateTime dt, string format = "dd/MM/yyyy HH:mm")
        {
            return dt.ToLocalTime().ToString(format);
        }
    }
}
