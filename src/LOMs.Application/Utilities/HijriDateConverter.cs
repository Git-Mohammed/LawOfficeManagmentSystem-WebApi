using System.Globalization;

namespace LOMs.Application.Utilities
{
    public static class HijriDateConverter
    {
        public static int GetCurrentHijriYear()
        {
            return new HijriCalendar().GetYear(DateTime.UtcNow) % 100;
        }
    }
}
