using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabuSearchProductionScheduling.Classes;

namespace TabuSearchProductionScheduling.Extensions
{
    public static class DateTimeExt
    {
        public static DateTime AddTime(this DateTime thisDateTime, double timeValue)
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(timeValue);
            return thisDateTime.Add(timeSpan);
        }
    }
}
