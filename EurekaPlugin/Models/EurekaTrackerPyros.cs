using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurekaPlugin.Models
{
    class EurekaTrackerPyros : IEurekaTracker
    {
        public Dictionary<int, EurekaMonster> monsters = new Dictionary<int, EurekaMonster>();

        const int PYROS_TERRITORY_TYPE_ID = 795;
        const int PYROS_MAP_ID = 484;

        public EurekaTrackerPyros(Dictionary<int, EurekaMonster> monsters)
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
            if (chance < 10)
            {
                return EurekaWeather.Fair_Skies;
            }
            else if (chance < 28)
            {
                return EurekaWeather.Heat_Waves;
            }
            else if (chance < 46)
            {
                return EurekaWeather.Thunder;
            }
            else if (chance < 64)
            {
                return EurekaWeather.Blizzards;
            }
            else if (chance < 82)
            {
                return EurekaWeather.Umbral_Wind;
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
            while (seenWeathers.Count < 6)
            {
                EurekaWeather weather = ChanceToWeather(Utilities.GetWeatherChance(i / 1000));
                result.Add(i, weather);
                i += eightHours;
                if (!seenWeathers.Contains(weather))
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
                if (result.Keys.Count >= 6)
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

        public static EurekaTrackerPyros GenerateNewPyrosTracker()
        {
            Dictionary<int, EurekaMonster> monsters = new Dictionary<int, EurekaMonster>();
            monsters.Add(38, new EurekaMonster(38,
                "Leucosia",
                "Medias Res",
                35,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                27,
                26,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                true,
                EurekaElement.Water,
                EurekaElement.Ice,
                null,
                null,
                "Pyros Bhoot"));

            monsters.Add(39, new EurekaMonster(39,
                "Flauros",
                "High Voltage",
                36,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                29,
                29,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.Thunder,
                false,
                false,
                EurekaElement.Lightning,
                EurekaElement.Lightning,
                null,
                null,
                "Thunderstorm Sprite"));

            monsters.Add(40, new EurekaMonster(40,
                "The Sophist",
                "On the Non-existent",
                37,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                31,
                31,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Wind,
                EurekaElement.Earth,
                null,
                null,
                "Pyros Apanda"));

            monsters.Add(41, new EurekaMonster(41,
                "Graffiacane",
                "Creepy Doll",
                38,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                23,
                37,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Ice,
                EurekaElement.Lightning,
                "Calca (minion)",
                null,
                "Valking"));

            monsters.Add(42, new EurekaMonster(42,
                "Askalaphos",
                "Quiet, Please",
                39,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                19,
                29,
                0,
                0,
                -1,
                false,
                EurekaWeather.Umbral_Wind,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Wind,
                EurekaElement.Earth,
                null,
                null,
                "Overdue Tome"));

            monsters.Add(43, new EurekaMonster(43,
                "Grand Duke Batym",
                "Up and Batym",
                40,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                18,
                14,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                true,
                EurekaElement.Earth,
                EurekaElement.Earth,
                null,
                null,
                "Dark Troubadour"));

            monsters.Add(44, new EurekaMonster(44,
                "Aetolus",
                "Rondo Aetolus",
                41,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                10,
                14,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Lightning,
                EurekaElement.Wind,
                null,
                null,
                "Islandhander"));

            monsters.Add(45, new EurekaMonster(45,
                "Lesath",
                "Scorchpion King",
                42,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                13,
                11,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Fire,
                EurekaElement.Wind,
                null,
                null,
                "Bird Eater"));

            monsters.Add(46, new EurekaMonster(46,
                "Eldthurs",
                "Burning Hunger",
                43,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                13,
                6,
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
                null,
                null,
                "Pyros Crab"));

            monsters.Add(47, new EurekaMonster(47,
                "Iris",
                "Dry Iris",
                44,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                21,
                11,
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
                "Northern Swallow"));

            monsters.Add(48, new EurekaMonster(48,
                "Lamebrix Strikebocks",
                "Thirty Whacks",
                45,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                21,
                7,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Earth,
                EurekaElement.Lightning,
                "Lamebrix's Dice (magicitie item)",
                null,
                "Illuminati Escapee"));

            monsters.Add(49, new EurekaMonster(49,
                "Dux",
                "Put Up Your Dux",
                46,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                27,
                9,
                0,
                0,
                -1,
                false,
                EurekaWeather.Thunder,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Lightning,
                EurekaElement.Fire,
                null,
                null,
                "Matanga Castaway"));

            monsters.Add(50, new EurekaMonster(50,
                "Lumber Jack",
                "You Do Know Jack",
                47,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                29,
                11,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Earth,
                EurekaElement.Lightning,
                "Wind-up Elvaan (minion)",
                null,
                "Pyros Treant"));

            monsters.Add(51, new EurekaMonster(51,
                "Glaukopis",
                "Mister Bright-eyes",
                48,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                31,
                15,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Fire,
                EurekaElement.Wind,
                null,
                null,
                "Val Skatene"));

            monsters.Add(52, new EurekaMonster(52,
                "Ying-Yang",
                "Haunter of the Dark",
                49,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                11,
                34,
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
                "Ying-Yang's Tissue (magicite item)",
                null,
                "Pyros Hecteyes"));

            monsters.Add(53, new EurekaMonster(53,
                "Skoll",
                "Heavens' Warg",
                50,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                23,
                29,
                0,
                0,
                -1,
                false,
                EurekaWeather.Blizzards,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Ice,
                EurekaElement.Earth,
                "Skoll's Claw (magicite item)",
                null,
                "Pyros Shuck"));

            monsters.Add(54, new EurekaMonster(54,
                "Penthesilea",
                "Lost Epic",
                50,
                PYROS_TERRITORY_TYPE_ID,
                PYROS_MAP_ID,
                35,
                6,
                0,
                0,
                -1,
                false,
                EurekaWeather.Heat_Waves,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Fire,
                EurekaElement.Fire,
                "Penthesilea Card (triple triad)",
                null,
                "Val Bloodglider"));

            return new EurekaTrackerPyros(monsters);
        }
    }
}
