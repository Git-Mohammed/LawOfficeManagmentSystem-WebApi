using System;
using System.Globalization;

namespace LOMs.Application.Utilities
{
    public static class HijriDateConverter
    {
        private static readonly HijriCalendar _calendar = new HijriCalendar();

        /// <summary>
        /// Gets the full current Hijri year (4 digits).
        /// Example: 1447
        /// </summary>
        public static int GetFullHijriYear()
        {
            return _calendar.GetYear(DateTime.UtcNow);
        }

        /// <summary>
        /// Gets the last 2 digits of the current Hijri year.
        /// Example: 1447 -> 47
        /// </summary>
        public static int GetCurrentHijriYearLastTwoDigits()
        {
            return GetYearLastDigits(GetFullHijriYear(), 2);
        }

        /// <summary>
        /// Gets the last digit of the current Hijri year.
        /// Example: 1447 -> 7
        /// </summary>
        public static int GetCurrentHijriYearLastDigit()
        {
            return GetYearLastDigits(GetFullHijriYear(), 1);
        }

        /// <summary>
        /// Gets the last N digits of a given Hijri year.
        /// Example: (1447, 2) -> 47
        /// </summary>
        public static int GetYearLastDigits(int year, int digits)
        {
            if (digits <= 0 || digits > 4)
                throw new ArgumentOutOfRangeException(nameof(digits), "Digits must be between 1 and 4.");

            int mod = (int)Math.Pow(10, digits);
            return year % mod;
        }

        /// <summary>
        /// Converts a Gregorian DateTime to its Hijri year.
        /// </summary>
        public static int ToHijriYear(DateTime gregorianDate)
        {
            return _calendar.GetYear(gregorianDate);
        }

        /// <summary>
        /// Converts a Hijri year into a string with optional formatting.
        /// Example: (1447, "yy") -> "47"
        /// </summary>
        public static string FormatHijriYear(int hijriYear, string format = "yyyy")
        {
            if (string.IsNullOrWhiteSpace(format))
                format = "yyyy";

            // Convert the Hijri year to a DateTime (1st Muharram of that year)
            var hijriDate = _calendar.ToDateTime(hijriYear, 1, 1, 0, 0, 0, 0);
            return hijriDate.ToString(format, new CultureInfo("ar-SA"));
        }
    }
}
