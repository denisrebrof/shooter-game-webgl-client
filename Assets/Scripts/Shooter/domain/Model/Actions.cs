using System;

namespace Shooter.domain.Model
{
    [Serializable]
    public struct ActionShoot
    {
        public long shooterId;
        public long weaponId;
        public TransformSnapshot direction;
    }
    
    [Serializable]
    public struct ActionJoinedStateChange
    {
        public long playerId;
        public bool joined;
    }

    [Serializable]
    public struct ActionHit
    {
        public long damagerId;
        public long receiverId;
        public int hpLoss;
        public bool killed;
    }
}