using DamageNumbersPro;
using UnityEngine;

namespace Shooter.presentation.Player.Weapons
{
    public class GamePopupsNavigator: MonoBehaviour
    {
        public DamageNumber numberPrefab;
        public DamageNumber killedPrefab;

        public void ShowDamage(Vector3 pos, int damage)
        {
            numberPrefab.Spawn(pos, damage);
        }
        
        public void ShowKilled(Vector3 pos)
        {
            killedPrefab.Spawn(pos, "Killed!");
        }
    }
}