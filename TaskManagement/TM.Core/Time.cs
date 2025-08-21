using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Core
{
    public class Time
    {
        public static TimeSpan ConverToTimeSpan(string time)
        {
            TimeSpan convertedTime;
            if (time.Equals(""))
            {
                DateTime t = DateTime.ParseExact("12:00:00", "hh:mm:ss", CultureInfo.InvariantCulture);
                convertedTime = t.TimeOfDay;
            }
            else if (time.Contains("AM") || time.Contains("PM"))
            {
                DateTime t = DateTime.ParseExact(time, "hh:mm tt", CultureInfo.InvariantCulture);
                convertedTime = t.TimeOfDay;
            }
            else if (time.Equals("00:00:00"))
            {
                DateTime t = DateTime.ParseExact("12:00:00", "hh:mm:ss", CultureInfo.InvariantCulture);
                convertedTime = t.TimeOfDay;
            }
            else
            {
                // DateTime t = DateTime.ParseExact(time, "HHmm", CultureInfo.InvariantCulture);
                try
                {
                    DateTime t = DateTime.ParseExact(time, "HH:mm:ss", CultureInfo.InvariantCulture);
                    convertedTime = t.TimeOfDay;
                }
                catch
                {
                    DateTime t = DateTime.ParseExact(time, "HH:mm:ss", CultureInfo.InvariantCulture);

                    convertedTime = t.TimeOfDay;
                }

            }
            return convertedTime;
        }

        public static string ConvertTo12HourFormat(string time)
        {
            string convertedTime;
            if (time.Equals(""))
            {
                convertedTime = "";
            }
            else if (time.Contains("AM") || time.Contains("PM"))
            {
                DateTime t = DateTime.ParseExact(time, "hh:mm tt", CultureInfo.InvariantCulture);
                convertedTime = t.TimeOfDay.ToString("hh:mm tt");
            }
            else if (time.Equals("00:00:00"))
            {
                DateTime dt = Convert.ToDateTime("01-Jan-2017 " + time);
                convertedTime = dt.ToString("hh:mm tt");
            }
            else
            {
                DateTime dt = Convert.ToDateTime("01-Jan-2017 " + time);
                convertedTime = dt.ToString("hh:mm tt");
            }
            return convertedTime;
        }
    }
}
