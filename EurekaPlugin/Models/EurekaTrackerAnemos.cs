using Dalamud.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurekaPlugin.Models
{
    class EurekaTrackerAnemos : IEurekaTracker
    {
        public Dictionary<int, EurekaMonster> monsters = new Dictionary<int, EurekaMonster>();

        const int ANEMOS_TERRITORY_TYPE_ID = 732;
        const int ANEMOS_MAP_ID = 414; // TODO: Should probaly be using lumina here

        public EurekaTrackerAnemos(Dictionary<int, EurekaMonster> monsters)
        {
            this.monsters = monsters;
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

        public Dictionary<int, EurekaMonster> GetMonsters()
        {
            return monsters;
        }

        public static EurekaTrackerAnemos GenerateNewAnemosTracker()
        {
            Dictionary<int, EurekaMonster> monsters = new Dictionary<int, EurekaMonster>();
            monsters.Add(1, new EurekaMonster(1,
                "Sabotender Corrido",
                "Unsafety Dance",
                1,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
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
                EurekaElement.Wind,
                EurekaElement.Wind,
                null,
                null,
                "Flowering Sabotender"));

            monsters.Add(2, new EurekaMonster(2,
                "The Lord of Anemos",
                "The Shadow over Anemos",
                2,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
                30,
                27,
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
                "The Prince of Anemos (minion)",
                null,
                "Sea Bishop"));

            monsters.Add(3, new EurekaMonster(3,
                "Teles",
                "Teles House",
                3,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
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
                EurekaElement.Wind,
                null,
                null,
                "Anemos Harpeia"));

            monsters.Add(4, new EurekaMonster(4,
                "The Emperor of Anemos",
                "The Swarm Never Sets",
                4,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
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
                EurekaElement.Wind,
                EurekaElement.Wind,
                "Emperor Hairpin (glamour)",
                null,
                "Darner"));

            monsters.Add(5, new EurekaMonster(5,
                "Callisto",
                "One Missed Callisto",
                5,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
                26,
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
                EurekaElement.Earth,
                null,
                null,
                "Val Bear"));

            monsters.Add(6, new EurekaMonster(6,
                "Number",
                "By Numbers",
                6,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
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
                EurekaElement.Lightning,
                EurekaElement.Lightning,
                null,
                null,
                "Pneumaflayer"));

            monsters.Add(7, new EurekaMonster(7,
                "Jahannam",
                "Disinherit the Wind",
                7,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
                18,
                19,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.Gales,
                false,
                false,
                EurekaElement.Wind,
                EurekaElement.Wind,
                null,
                null,
                "Typhoon Sprite"));

            monsters.Add(8, new EurekaMonster(8,
                "Amemet",
                "Prove Your Amemettle",
                8,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
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
                EurekaElement.Fire,
                null,
                null,
                "Abraxas"));

            monsters.Add(9, new EurekaMonster(9,
                "Caym",
                "Caym What May",
                9,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
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
                EurekaElement.Ice,
                EurekaElement.Ice,
                null,
                null,
                "Stalker Ziz"));

            monsters.Add(10, new EurekaMonster(10,
                "Bombadeel",
                "The Killing of a Sacred Bombadier",
                10,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
                28,
                20,
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
                "Traveling Gourmand"));

            monsters.Add(11, new EurekaMonster(11,
                "Serket",
                "Short Serket 2",
                11,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
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
                EurekaElement.Earth,
                "Scorpion Harness (glamour)",
                "Wind-up Mithra (minion)",
                "Khor Claw"));

            monsters.Add(12, new EurekaMonster(12,
                "Judgmental Julika",
                "Don't Judge Me, Morbol",
                12,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
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
                EurekaElement.Ice,
                EurekaElement.Ice,
                null,
                null,
                "Henbane"));

            monsters.Add(13, new EurekaMonster(13,
                "The White Rider",
                "When You Ride Alone",
                13,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
                20,
                13,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                true,
                EurekaElement.Lightning,
                EurekaElement.Fire,
                null,
                null,
                "Duskfall Dullahan"));

            monsters.Add(14, new EurekaMonster(14,
                "Polyphemus",
                "Sing, Muse",
                14,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
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
                EurekaElement.Ice,
                EurekaElement.Ice,
                null,
                null,
                "Monoeye"));

            monsters.Add(15, new EurekaMonster(15,
                "Simurgh's Strider",
                "Simurghasbord",
                15,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
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
                EurekaElement.Fire,
                "Strider Boots (sprint +10s in city states)",
                null,
                "Old World Zu"));

            monsters.Add(16, new EurekaMonster(16,
                "King Hazmat",
                "To the Mat",
                16,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
                35,
                18,
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
                "Anemos Anala"));

            monsters.Add(17, new EurekaMonster(17,
                "Fafnir",
                "Wine and Honey",
                17,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
                36,
                22,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                true,
                EurekaElement.Fire,
                EurekaElement.Fire,
                "Wind-up Fafnir (minion)",
                null,
                "Fossil Dragon"));

            monsters.Add(18, new EurekaMonster(18,
                "Amarok",
                "I Amarok",
                18,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
                8,
                18,
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
                "Voidscale"));

            monsters.Add(19, new EurekaMonster(19,
                "Lamashtu",
                "Drama Lamashtu",
                19,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
                8,
                23,
                0,
                0,
                -1,
                false,
                EurekaWeather.None,
                EurekaWeather.None,
                false,
                true,
                EurekaElement.Wind,
                EurekaElement.Wind,
                null,
                null,
                "Val Specter"));

            monsters.Add(20, new EurekaMonster(20,
                "Pazuzu",
                "Wail in the Willows",
                20,
                ANEMOS_TERRITORY_TYPE_ID,
                ANEMOS_MAP_ID,
                7,
                22,
                0,
                0,
                -1,
                false,
                EurekaWeather.Gales,
                EurekaWeather.None,
                false,
                true,
                EurekaElement.Wind,
                EurekaElement.Fire,
                "Altar to Pazuzu (tabletop)",
                "Pazuzu Card (triple triad)",
                "Shadow Wraith"));

            return new EurekaTrackerAnemos(monsters);
        }

        public EurekaWeather ChanceToWeather(int chance)
        {
            if (chance < 30)
            {
                return EurekaWeather.Fair_Skies;
            }
            else if (chance < 60)
            {
                return EurekaWeather.Gales;
            }
            else if (chance < 90)
            {
                return EurekaWeather.Showers;
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
            while (seenWeathers.Count < 4)
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
                if (result.Keys.Count >= 4)
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
    }
}
