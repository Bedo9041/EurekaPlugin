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

        const int PAGOS_TERRITORY_TYPE_ID = 763;
        const int PAGOS_MAP_ID = 467;

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
                "Eternity",
                20,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                21,
                26,
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
                "Cairn Blight 451",
                21,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                25,
                28,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                true,
                EurekaElement.Earth,
                EurekaElement.Wind,
                null,
                null,
                "Demon of the Incunable"));

            monsters.Add(23, new EurekaMonster(23,
                "Ash Dragon",
                "Ash the Magic Dragon",
                22,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                29,
                30,
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
                "Blood Demon"));

            monsters.Add(24, new EurekaMonster(24,
                "Glavoid",
                "Conqueror Worm",
                23,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                32,
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
                EurekaElement.Earth,
                null,
                null,
                "Val Worm"));

            monsters.Add(25, new EurekaMonster(25,
                "Anapos",
                "Melting Point",
                24,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                34,
                21,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.Fog,
                false,
                false,
                EurekaElement.Water,
                EurekaElement.Water,
                null,
                null,
                "Snowmelt Sprite"));

            monsters.Add(26, new EurekaMonster(26,
                "Hakutaku",
                "The Wobbler in Darkness",
                25,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                29,
                22,
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
                "Does It Have to Be a Snowman",
                26,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                17,
                16,
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
                null,
                null,
                "Huwasi"));

            monsters.Add(28, new EurekaMonster(28,
                "Asag",
                "Disorder in the Court",
                27,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                10,
                10,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Lightning,
                EurekaElement.Lightning,
                null,
                null,
                "Wandering Opken"));

            monsters.Add(29, new EurekaMonster(29,
                "Surabhi",
                "Cows for Concern",
                28,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                10,
                20,
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
                "Pagos Billygoat"));

            monsters.Add(30, new EurekaMonster(30,
                "King Arthro",
                "Morte Arthro",
                29,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                8,
                15,
                0,
                0,
                -1,
                false,
                EurekaWeather.Fog,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Water,
                EurekaElement.Water,
                "Speed Belt (eureka effect)",
                null,
                "Val Snipper"));

            monsters.Add(31, new EurekaMonster(31,
                "Mindertaur/Eldertaur",
                "Brothers",
                30,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                13,
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
                EurekaElement.Wind,
                null,
                null,
                "Lab Minotaur"));

            monsters.Add(32, new EurekaMonster(32,
                "Holy Cow",
                "Apocalypse Cow",
                31,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                26,
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
                EurekaElement.Wind,
                null,
                null,
                "Elder Buffalo"));

            monsters.Add(33, new EurekaMonster(33,
                "Hadhayosh",
                "Third Impact",
                32,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                30,
                19,
                0,
                0,
                -1,
                false,
                EurekaWeather.Thunder,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Lightning,
                EurekaElement.Lightning,
                "Behemoth Horn (to buy Behemoth gear)",
                "Behemoth Pelt (to buy Behemoth gear)",
                "Lesser Void Dragon"));

            monsters.Add(34, new EurekaMonster(34,
                "Horus",
                "Eye of Horus",
                33,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                25,
                19,
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
                null,
                null,
                "Void Vouivre"));

            monsters.Add(35, new EurekaMonster(35,
                "Arch Angra Mainyu",
                "Eye Scream for Ice Cream",
                34,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                24,
                25,
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
                "Gawper"));

            monsters.Add(36, new EurekaMonster(36,
                "Copycat Cassie",
                "Cassie and the Copycats",
                35,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                22,
                14,
                0,
                0,
                -1,
                false,
                EurekaWeather.Blizzards,
                EurekaWeather.None,
                false,
                false,
                EurekaElement.Ice,
                EurekaElement.Ice,
                "Cassie Earring (eureka effect)",
                null,
                "Ameretat"));

            monsters.Add(37, new EurekaMonster(37,
                "Louhi",
                "Louhi on Ice",
                35,
                PAGOS_TERRITORY_TYPE_ID,
                PAGOS_MAP_ID,
                36,
                19,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                true,
                EurekaElement.Ice,
                EurekaElement.Ice,
                "Louhi Card (triple triad)",
                null,
                "Val Corpse"));

            return new EurekaTrackerPagos(monsters);
        }
    }
}
