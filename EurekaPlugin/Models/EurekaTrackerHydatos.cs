using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurekaPlugin.Models
{
    class EurekaTrackerHydatos : IEurekaTracker
    {
        public Dictionary<int, EurekaMonster> monsters = new Dictionary<int, EurekaMonster>();

        const int HYDATOS_TERRITORY_TYPE_ID = 827;
        const int HYDATOS_MAP_ID = 515;

        public EurekaTrackerHydatos(Dictionary<int, EurekaMonster> monsters)
        {
            this.monsters = monsters;
        }

        public Dictionary<int, EurekaMonster> GetMonsters()
        {
            return monsters;
        }

        public void SetKilledTimes(Dictionary<int, long> killTimes)
        {
            foreach (int monsterId in monsters.Keys)
            {
                if (killTimes.ContainsKey(monsterId))
                {
                    monsters[monsterId].SetKilledAt(killTimes[monsterId]);
                }
                else
                {
                    monsters[monsterId].ResetKilled();
                }
            }
        }

        public void SetPreppedList(Dictionary<int, bool> preppedList)
        {
            foreach (int monsterId in monsters.Keys)
            {
                if (preppedList.ContainsKey(monsterId))
                {
                    monsters[monsterId].SetPrepped(preppedList[monsterId]);
                }
                else
                {
                    monsters[monsterId].SetPrepped(false);
                }
            }
        }

        public EurekaWeather ChanceToWeather(int chance)
        {
            if (chance < 12)
            {
                return EurekaWeather.Fair_Skies;
            }
            else if (chance < 34)
            {
                return EurekaWeather.Showers;
            }
            else if (chance < 56)
            {
                return EurekaWeather.Gloom;
            }
            else if (chance < 78)
            {
                return EurekaWeather.Thunderstorms;
            }
            return EurekaWeather.Snow;
        }

        public Dictionary<long, EurekaWeather> CalculateNextWeathers(int amountIn)
        {
            int amount = amountIn + 1;
            var eightHours = 175 * 1000 * 8;
            var msec = Utilities.CurrentTimestampMilliseconds();
            var toRemove = msec % eightHours;
            var startMsec = msec - toRemove;
            Dictionary<long, EurekaWeather> result = new Dictionary<long, EurekaWeather>();
            for (long i = startMsec + eightHours; i < (startMsec + (amount * eightHours)); i += eightHours)
            {
                result.Add(i, ChanceToWeather(Utilities.GetWeatherChance(i / 1000)));
            }
            return result;
        }

        public Dictionary<long, EurekaWeather> CalculateNextWeathersUntilAllFound()
        {
            List<EurekaWeather> seenWeathers = new List<EurekaWeather>();
            var eightHours = 175 * 1000 * 8;
            var msec = Utilities.CurrentTimestampMilliseconds();
            var toRemove = msec % eightHours;
            var startMsec = msec - toRemove;
            Dictionary<long, EurekaWeather> result = new Dictionary<long, EurekaWeather>();
            long i = startMsec + eightHours;
            while (seenWeathers.Count < 5)
            {
                EurekaWeather weather = ChanceToWeather(Utilities.GetWeatherChance(i / 1000));
                result.Add(i, weather);
                i += eightHours;
                if(!seenWeathers.Contains(weather))
                {
                    seenWeathers.Add(weather);
                }
            }
            return result;
        }

        public Dictionary<EurekaWeather, TimeSpan> GetTimeUntilWeathers()
        {
            Dictionary<long, EurekaWeather> weathers = CalculateNextWeathersUntilAllFound();
            Dictionary<EurekaWeather, TimeSpan> result = new Dictionary<EurekaWeather, TimeSpan>();
            foreach (long time in weathers.Keys)
            {
                if (result.Keys.Count >= 5)
                    break;
                if (result.ContainsKey(weathers[time]))
                    continue;
                result.Add(weathers[time], TimeSpan.FromMilliseconds(time - Utilities.CurrentTimestampMilliseconds()));
            }
            return result;
        }

        public EurekaWeather GetCurrentWeather()
        {
            return ChanceToWeather(Utilities.GetWeatherChance(Utilities.CurrentTimestampSeconds()));
        }

        public static EurekaTrackerHydatos GenerateNewHydatosTracker()
        {
            Dictionary<int, EurekaMonster> monsters = new Dictionary<int, EurekaMonster>();
            monsters.Add(55, new EurekaMonster(55,
                "Khalamari",
                "I Ink, Therefore I Am",
                50,
                HYDATOS_TERRITORY_TYPE_ID,
                HYDATOS_MAP_ID,
                11,
                24,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Water,
                EurekaElement.Water,
                null,
                null,
                "Xzomit"));

            monsters.Add(56, new EurekaMonster(56,
                "Stegodon",
                "From Tusk till Dawn",
                51,
                HYDATOS_TERRITORY_TYPE_ID,
                HYDATOS_MAP_ID,
                9,
                17,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Earth,
                EurekaElement.Earth,
                null,
                null,
                "Hydatos Primelephas"));

            monsters.Add(57, new EurekaMonster(57,
                "Molech",
                "Bullheaded Berserker",
                52,
                HYDATOS_TERRITORY_TYPE_ID,
                HYDATOS_MAP_ID,
                7,
                21,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Ice,
                EurekaElement.Earth,
                "Molech's Horn (magicite item)",
                null,
                "Val Nullchu"));

            monsters.Add(58, new EurekaMonster(58,
                "Piasa",
                "Mad, Bad, and Fabulous to Know",
                53,
                HYDATOS_TERRITORY_TYPE_ID,
                HYDATOS_MAP_ID,
                7,
                14,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Wind,
                EurekaElement.Wind,
                null,
                null,
                "Vivid Gastornis"));

            monsters.Add(59, new EurekaMonster(59,
                "Frostmane",
                "Fearful Symmetry",
                54,
                HYDATOS_TERRITORY_TYPE_ID,
                HYDATOS_MAP_ID,
                8,
                25,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Fire,
                EurekaElement.Earth,
                null,
                null,
                "Northern Tiger"));

            monsters.Add(60, new EurekaMonster(60,
                "Daphne",
                "Crawling Chaos",
                55,
                HYDATOS_TERRITORY_TYPE_ID,
                HYDATOS_MAP_ID,
                25,
                15,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                true,
                EurekaElement.Water,
                EurekaElement.Water,
                null,
                null,
                "Dark Void Monk"));

            monsters.Add(61, new EurekaMonster(61,
                "King Goldemar",
                "Duty-free",
                56,
                HYDATOS_TERRITORY_TYPE_ID,
                HYDATOS_MAP_ID,
                28,
                23,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                true,
                EurekaElement.Lightning,
                EurekaElement.Lightning,
                "Goldemar's Horn (magicite item)",
                "Dverge Card (triple triad)",
                "Hydatos Wraith"));

            monsters.Add(62, new EurekaMonster(62,
                "Leuke",
                "Leukewarm Reception",
                57,
                HYDATOS_TERRITORY_TYPE_ID,
                HYDATOS_MAP_ID,
                37,
                26,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Earth,
                EurekaElement.Wind,
                null,
                null,
                "Tigerhawk"));

            monsters.Add(63, new EurekaMonster(63,
                "Barong",
                "Robber Barong",
                58,
                HYDATOS_TERRITORY_TYPE_ID,
                HYDATOS_MAP_ID,
                32,
                24,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Fire,
                EurekaElement.Earth,
                null,
                null,
                "Laboratory Lion"));

            monsters.Add(64, new EurekaMonster(64,
                "Ceto",
                "Stone-col Killer",
                59,
                HYDATOS_TERRITORY_TYPE_ID,
                HYDATOS_MAP_ID,
                36,
                13,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Water,
                EurekaElement.Fire,
                "Ceto's Claw (magicite item)",
                null,
                "Hydatos Delphyne"));

            monsters.Add(65, new EurekaMonster(65,
                "Provenance Watcher",
                "Crystalline Provenance",
                60,
                HYDATOS_TERRITORY_TYPE_ID,
                HYDATOS_MAP_ID,
                32,
                19,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Fire,
                EurekaElement.Fire,
                "Provenance Watcher Card (triple triad)",
                null,
                "Crystal Claw"));

            return new EurekaTrackerHydatos(monsters);
        }
    }
}
