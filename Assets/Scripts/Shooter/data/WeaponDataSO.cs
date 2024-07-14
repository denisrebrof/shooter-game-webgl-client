using System;
using Shooter.domain.Repositories;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Shooter.data
{
    [CreateAssetMenu(fileName = "WeaponPrefabs", menuName = "WeaponPrefabs")]
    public class WeaponDataSO : ScriptableObject, IWeaponIconRepository
    {
        [SerializeField] private SerializableDictionary<long, WeaponData> data;
        [SerializeField] private WeaponData defaultData;

        public Sprite GetIcon(long weaponId) => GetData(weaponId).icon;

        public WeaponData GetData(long id) => data
            .TryGetValue(id, out var weaponData)
            ? weaponData
            : defaultData;
    }

    [Serializable]
    public struct WeaponData
    {
        public AssetReferenceGameObject otherPrefab;
        public AssetReferenceGameObject lobbyPrefab;
        public AssetReferenceGameObject playerPrefab;
        public Sprite icon;
    }
}