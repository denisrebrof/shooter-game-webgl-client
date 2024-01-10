using UnityEngine;
using Utils.Pooling;

namespace Shooter.presentation.Player.Weapons
{
    public class SimpleBulletPool: MonoPool<SimpleBullet>
    {
        [ContextMenu("Generate")]
        public void GeneratePool() => Generate();
    }
}