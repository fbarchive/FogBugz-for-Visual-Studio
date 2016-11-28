using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FogBugzForVisualStudio.Api
{
    public class Estimate
    {
        public decimal hrs { get; private set; }

        public Estimate(decimal hrs)
        {
            this.hrs = hrs;
        }

        public static Estimate Parse(String s) {
            return new Estimate(Convert.ToDecimal(s, CultureInfo.InvariantCulture));
        }
        
        public override string ToString()
        {
            if (hrs == 0) return "";

            // Based on SFromHours in util.was
            int nHours = Convert.ToInt32(Decimal.Floor(hrs));
            int nMinutes = Convert.ToInt32(Decimal.Floor((hrs - nHours) * 60));

            if (nHours == 0 && nMinutes == 0)
            {
                return "0 hours";
            }
            else if (nHours == 0)
            {
                return nMinutes == 1 ? "1 minute" : (nMinutes + " minutes");
            }
            else
            {
                double h = Math.Round(nHours + nMinutes/60.0, 2);
                return h == 1 ? "1 hour" : (h + " hours");
            }
        }
    }
}
