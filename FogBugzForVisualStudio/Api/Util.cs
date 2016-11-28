using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace FogBugzForVisualStudio.Api
{
    static class Util
    {
        public static Nullable<DateTime> ParseApiDate(string s)
        {
            // Shortcut for empty strings
            if (String.IsNullOrEmpty(s)) return null;
            
            try
            {
                return DateTime.ParseExact(s, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToLocalTime();
            }
            catch
            {
                return null;
            }
        }

        public static string FormatApiDate(DateTime dt)
        {
            return dt.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }
}
