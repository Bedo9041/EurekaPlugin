using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurekaPlugin.Models
{
    interface IEurekaTracker
    {
        void SetKilledTimes(Dictionary<int, long> killTimes);

        void SetPreppedList(Dictionary<int, bool> preppedList);

        Dictionary<int, EurekaMonster> GetMonsters();

        EurekaWeather ChanceToWeather(int chance);

        Dictionary<long, EurekaWeather> CalculateNextWeathers(int amount);

        Dictionary<EurekaWeather, TimeSpan> GetTimeUntilWeathers();

        EurekaWeather GetCurrentWeather();
    }
}
