using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurekaPlugin.Models
{
    class EurekaTrackerPagos : IEurekaTracker
    {
        public Dictionary<int, EurekaMonster> monsters = new Dictionary<int, EurekaMonster>();

        public EurekaTrackerPagos(Dictionary<int, EurekaMonster> monsters)
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
                return EurekaWeather.Fog;
            }
            else if (chance < 46)
            {
                return EurekaWeather.Heat_Waves;
            }
            else if (chance < 64)
            {
                return EurekaWeather.Snow;
            }
            else if (chance < 82)
            {
                return EurekaWeather.Thunder;
            }
            return EurekaWeather.Blizzards;
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

        public static EurekaTrackerPagos GenerateNewPagosTracker()
        {
            Dictionary<int, EurekaMonster> monsters = new Dictionary<int, EurekaMonster>();
            monsters.Add(21, new EurekaMonster(21,
                "The Snow Queen",
                "",
                20,
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
                false,
                EurekaElement.Ice,
                EurekaElement.Ice,
                "Yukinko Card (triple triad)",
                null,
                "Yukinko"));

            monsters.Add(22, new EurekaMonster(22,
                "Taxim",
                "",
                21,
                732,
                414,
                30,
                27,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                true,
                EurekaElement.Earth,
                EurekaElement.Earth, // TODO
                null,
                null,
                "Demon of the Incunable"));

            monsters.Add(23, new EurekaMonster(23,
                "Ash Dragon",
                "",
                22,
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
                EurekaElement.Fire,
                EurekaElement.Fire, // TODO
                null,
                null,
                "Blood Demon"));

            monsters.Add(24, new EurekaMonster(24,
                "Glavoid",
                "",
                23,
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
                EurekaElement.Earth,
                EurekaElement.Earth, // TODO
                null,
                null,
                "Val Worm"));

            monsters.Add(25, new EurekaMonster(25,
                "Anapos",
                "",
                24,
                732,
                414,
                26,
                22,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.Fog,
                false,
                false,
                EurekaElement.Water,
                EurekaElement.Water, // TODO
                null,
                null,
                "Snowmelt Sprite"));

            monsters.Add(26, new EurekaMonster(26,
                "Hakutaku",
                "",
                25,
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
                false,
                EurekaElement.Fire,
                EurekaElement.Fire,
                "Optical Hat (eureka effect, gold required)",
                null,
                "Blubber Eyes"));

            monsters.Add(27, new EurekaMonster(27,
                "King Igloo",
                "",
                26,
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
                EurekaElement.Ice,
                EurekaElement.Ice, // TODO
                null,
                null,
                "Huwasi"));

            monsters.Add(28, new EurekaMonster(28,
                "Asag",
                "",
                27,
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
                EurekaElement.Lightning,
                EurekaElement.Lightning, // TODO
                null,
                null,
                "Wandering Opken"));

            monsters.Add(29, new EurekaMonster(29,
                "Surabhi",
                "",
                28,
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
                EurekaElement.Earth,
                EurekaElement.Earth, // TODO
                null,
                null,
                "Pagos Billygoat"));

            monsters.Add(30, new EurekaMonster(30,
                "King Arthro",
                "",
                29,
                732,
                414,
                28,
                20,
                0,
                0,
                -1,
                false,
                EurekaWeather.Fog,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Water,
                EurekaElement.Water, // TODO
                "Speed Belt (eureka effect)",
                null,
                "Val Snipper"));

            monsters.Add(31, new EurekaMonster(31,
                "Mindertaur/Eldertaur",
                "",
                30,
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
                null,
                null,
                "Lab Minotaur"));

            monsters.Add(32, new EurekaMonster(32,
                "Holy Cow",
                "",
                31,
                732,
                414,
                22,
                16,
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
                "Elder Buffalo"));

            monsters.Add(33, new EurekaMonster(33,
                "Hadhayosh",
                "",
                32,
                732,
                414,
                20,
                13,
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
                "Behemoth Horn (to buy Behemoth gear)",
                "Behemoth Pelt (to buy Behemoth gear)",
                "Lesser Void Dragon"));

            monsters.Add(34, new EurekaMonster(34,
                "Horus",
                "",
                33,
                732,
                414,
                26,
                14,
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
                null,
                null,
                "Void Vouivre"));

            monsters.Add(35, new EurekaMonster(35,
                "Arch Angra Mainyu",
                "",
                34,
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
                EurekaElement.Wind,
                EurekaElement.Wind, // TODO
                null,
                null,
                "Gawper"));

            monsters.Add(36, new EurekaMonster(36,
                "Copycat Cassie",
                "",
                35,
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
                "Cassie Earring (eureka effect)",
                null,
                "Ameretat"));

            monsters.Add(37, new EurekaMonster(37,
                "Louhi",
                "",
                35,
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
                true,
                EurekaElement.Ice,
                EurekaElement.Ice, // TODO
                "Louhi Card (triple triad)",
                null,
                "Val Corpse"));

            return new EurekaTrackerPagos(monsters);
        }
    }
}
