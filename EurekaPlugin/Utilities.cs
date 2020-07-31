using Dalamud.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurekaPlugin
{
    public static class Utilities
    {
        public static DateTime GetEorzeaTime()
        {
            return ToEorzeaTime(DateTime.Now);
        }

        public static DateTime ToEorzeaTime(this DateTime date)
        {
            const double MULTIPLER = 144D / 7D;
            long epochTicks = date.ToUniversalTime().Ticks - (new DateTime(1970, 1, 1).Ticks);
            long eorzeaTicks = (long)Math.Round(epochTicks * MULTIPLER);
            return new DateTime(eorzeaTicks);
        }

        public static TimeSpan EorzeaTimeSpanToRealTimeSpan(TimeSpan timeSpan)
        {
            const double MULTIPLER = 7D / 144D;
            return TimeSpan.FromTicks(Convert.ToInt64(timeSpan.Ticks * MULTIPLER));
        }

        public static TimeSpan TimeUntilEorzeaDay()
        {
            DateTime nextNight;
            if (GetEorzeaTime().Hour < 6)
            {
                TimeSpan ts = new TimeSpan(6, 0, 0);
                nextNight = GetEorzeaTime().Date + ts;
            }
            else
            {
                TimeSpan ts = new TimeSpan(1, 6, 0, 0);
                nextNight = GetEorzeaTime().Date + ts;
            }
            return nextNight - GetEorzeaTime();
        }

        public static TimeSpan TimeUntilEorzeaNight()
        {
            DateTime nextNight;
            if (GetEorzeaTime().Hour < 19)
            {
                TimeSpan ts = new TimeSpan(19, 0, 0);
                nextNight = GetEorzeaTime().Date + ts;
            }
            else
            {
                TimeSpan ts = new TimeSpan(1, 19, 0, 0);
                nextNight = GetEorzeaTime().Date + ts;
            }
            return nextNight - GetEorzeaTime();
        }

        public static long CurrentTimestampMilliseconds()
        {
            return DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static long CurrentTimestampSeconds()
        {
            return DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).Ticks / TimeSpan.TicksPerSecond;
        }

        public static int GetWeatherChance(long timestampSeconds)
        {
            var bell = timestampSeconds / 175;
            var increment = ((uint)(bell + 8 - (bell % 8))) % 24;
            var totalDays = (uint)(timestampSeconds / 4200);
            var calcBase = (totalDays * 0x64) + increment;
            var step1 = (calcBase << 0xB) ^ calcBase;
            var step2 = (step1 >> 8) ^ step1;
            return (int)(step2 % 0x64);
        }
    }
}
