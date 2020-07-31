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
                "",
                35,
                732,
                414,
                14,
                22,
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
                "Pyros Bhoot"));

            monsters.Add(39, new EurekaMonster(39,
                "Flauros",
                "",
                36,
                732,
                414,
                30,
                27,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.Thunder,
                false,
                false,
                EurekaElement.Lightning,
                EurekaElement.Lightning, // TODO
                null,
                null,
                "Thunderstorm Sprite"));

            monsters.Add(40, new EurekaMonster(40,
                "The Sophist",
                "",
                37,
                732,
                414,
                26,
                28,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Wind,
                EurekaElement.Wind, // TODO
                null,
                null,
                "Pyros Apanda"));

            monsters.Add(41, new EurekaMonster(41,
                "Graffiacane",
                "",
                38,
                732,
                414,
                17,
                22,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Ice,
                EurekaElement.Ice, // TODO
                "Calca (minion)",
                null,
                "Valking"));

            monsters.Add(42, new EurekaMonster(42,
                "Askalaphos",
                "",
                39,
                732,
                414,
                26,
                22,
                0,
                0,
                -1,
                false,
                EurekaWeather.Umbral_Wind,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Wind,
                EurekaElement.Wind, // TODO
                null,
                null,
                "Overdue Tome"));

            monsters.Add(43, new EurekaMonster(43,
                "Grand Duke Batym",
                "",
                40,
                732,
                414,
                24,
                23,
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
                "",
                41,
                732,
                414,
                18,
                19,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Lightning,
                EurekaElement.Lightning, // TODO
                null,
                null,
                "Islandhander"));

            monsters.Add(45, new EurekaMonster(45,
                "Lesath",
                "",
                42,
                732,
                414,
                15,
                16,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Fire,
                EurekaElement.Fire, // TODO
                null,
                null,
                "Bird Eater"));

            monsters.Add(46, new EurekaMonster(46,
                "Eldthurs",
                "",
                43,
                732,
                414,
                14,
                13,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Fire,
                EurekaElement.Fire, // TODO
                null,
                null,
                "Pyros Crab"));

            monsters.Add(47, new EurekaMonster(47,
                "Iris",
                "",
                44,
                732,
                414,
                28,
                20,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Water,
                EurekaElement.Water, // TODO
                null,
                null,
                "Northern Swallow"));

            monsters.Add(48, new EurekaMonster(48,
                "Lamebrix Strikebocks",
                "",
                45,
                732,
                414,
                25,
                18,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Earth,
                EurekaElement.Earth, // TODO
                "Lamebrix's Dice (magicitie item)",
                null,
                "Illuminati Escapee"));

            monsters.Add(49, new EurekaMonster(49,
                "Dux",
                "",
                46,
                732,
                414,
                22,
                16,
                0,
                0,
                -1,
                false,
                EurekaWeather.Thunder,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Lightning,
                EurekaElement.Lightning, // TODO
                null,
                null,
                "Matanga Castaway"));

            monsters.Add(50, new EurekaMonster(50,
                "Lumber Jack",
                "",
                47,
                732,
                414,
                20,
                13,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Earth,
                EurekaElement.Earth, // TODO
                "Wind-up Elvaan (minion)",
                null,
                "Pyros Treant"));

            monsters.Add(51, new EurekaMonster(51,
                "Glaukopis",
                "",
                48,
                732,
                414,
                26,
                14,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Fire,
                EurekaElement.Fire, // TODO
                null,
                null,
                "Val Skatene"));

            monsters.Add(52, new EurekaMonster(52,
                "Ying-Yang",
                "",
                49,
                732,
                414,
                29,
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
                EurekaElement.Water, // TODO
                "Ying-Yang's Tissue (magicite item)",
                null,
                "Pyros Hecteyes"));

            monsters.Add(53, new EurekaMonster(53,
                "Skoll",
                "",
                50,
                732,
                414,
                29,
                13,
                0,
                0,
                -1,
                false,
                EurekaWeather.Blizzards,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Ice,
                EurekaElement.Ice, // TODO
                "Skoll's Claw (magicite item)",
                null,
                "Pyros Shuck"));

            monsters.Add(54, new EurekaMonster(54,
                "Penthesilea",
                "",
                50,
                732,
                414,
                29,
                13,
                0,
                0,
                -1,
                false,
                EurekaWeather.Heat_Waves,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Fire,
                EurekaElement.Fire, // TODO
                "Penthesilea Card (triple triad)",
                null,
                "Val Bloodglider"));

            return new EurekaTrackerPyros(monsters);
        }
    }
}
