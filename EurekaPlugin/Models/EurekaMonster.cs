using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurekaPlugin.Models
{
    class EurekaMonster
    {
        public int MonsterId;
        public string MonsterName;
        public string FateName;
        public int FateLevel;
        public uint TerritoryTypeId;
        public uint MapId;
        public float XPos;
        public float YPos;
        public float SpawnMobXPos; // TODO: Multiple Spawn Mob Locations
        public float SpawnMobYPos;
        public long KilledAt;
        public bool IsPrepped;
        public EurekaWeather SpawnRequiredWeather;
        public EurekaWeather SpawnMobRequiredWeather;
        public bool SpawnRequiredNight;
        public bool SpawnMobRequiredNight;
        public EurekaElement MonsterElement;
        public EurekaElement SpawnMobElement;
        public string RewardOne;
        public string RewardTwo;
        public string SpawnMobName;

        public EurekaMonster(int monsterId, string monsterName, string fateName, int fateLevel, uint territoryTypeId, uint mapId, float xPos, float yPos, float spawnMobXPos, float spawnMobYPos, long killedAt, bool isPrepped, EurekaWeather spawnRequiredWeather, EurekaWeather spawnMobRequiredWeather, bool spawnRequiredNight, bool spawnMobRequiredNight, EurekaElement monsterElement, EurekaElement spawnMobElement, string rewardOne, string rewardTwo, string spawnMobName)
        {
            MonsterId = monsterId;
            MonsterName = monsterName;
            FateName = fateName;
            FateLevel = fateLevel;
            TerritoryTypeId = territoryTypeId;
            MapId = mapId;
            XPos = xPos;
            YPos = yPos;
            SpawnMobXPos = spawnMobXPos;
            SpawnMobYPos = spawnMobYPos;
            KilledAt = killedAt;
            IsPrepped = isPrepped;
            SpawnRequiredWeather = spawnRequiredWeather;
            SpawnMobRequiredWeather = spawnMobRequiredWeather;
            SpawnRequiredNight = spawnRequiredNight;
            SpawnMobRequiredNight = spawnMobRequiredNight;
            MonsterElement = monsterElement;
            SpawnMobElement = spawnMobElement;
            RewardOne = rewardOne;
            RewardTwo = rewardTwo;
            SpawnMobName = spawnMobName;
        }

        public bool IsKilled()
        {
            return KilledAt != -1 && (KilledAt + 7200000) > Utilities.CurrentTimestampMilliseconds();
        }

        public void ResetKilled()
        {
            KilledAt = -1;
        }

        public TimeSpan GetTimeUntilReset()
        {
            return TimeSpan.FromMilliseconds(KilledAt + 7200000 - Utilities.CurrentTimestampMilliseconds());
        }

        public DateTime GetSpawnTime()
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(KilledAt).ToLocalTime();
        }

        public bool HasSpawnTime()
        {
            return KilledAt == -1;
        }

        public void SetKilledAt(long time)
        {
            KilledAt = time;
        }

        public void SetPrepped(bool prepped)
        {
            IsPrepped = prepped;
        }
    }
}
