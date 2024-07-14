using System;

namespace Shooter.domain.Model
{
    [Serializable]
   public struct LevelProgressionData
    {
        public int level;
        public int xp;
        public int nextLevelXp;
    }
}