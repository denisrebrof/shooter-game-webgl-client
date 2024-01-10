using System;
using Shooter.domain;
using Shooter.domain.Repositories;
using UnityEngine;

namespace Shooter.presentation.Player
{
    [CreateAssetMenu(fileName = "WeaponPrefabs", menuName = "WeaponPrefabs")]
    public class WeaponDataSO : ScriptableObject, IWeaponDamageRepository
    {
        [SerializeField] private SerializableDictionary<long, WeaponData> data;
        [SerializeField] private WeaponData defaultData;

        public WeaponData GetData(long id) => data
            .TryGetValue(id, out var weaponData)
            ? weaponData
            : defaultData;

        public int GetDamage(long weaponId) => GetData(weaponId).damage;
    }

    [Serializable]
    public struct WeaponData
    {
        public GameObject prefab;
        public GameObject playerPrefab;
        public Sprite icon;
        public int damage;
    }
    
    
}