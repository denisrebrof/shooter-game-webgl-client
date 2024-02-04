using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Player.Weapons
{
    public class PlayerWeaponsController : PlayerDataBehaviour
    {
        [SerializeField] private Transform weaponRoot;

        [Inject] private WeaponDataSO weaponData;

        private IDisposable handler = Disposable.Empty;

        private void OnEnable()
        {
            handler = PlayerDataFlow
                .Select(data => data.selectedWeaponId)
                .DistinctUntilChanged()
                .Select(id => weaponData.GetData(id).prefab)
                .Subscribe(SetWeapon)
                .AddTo(this);
        }

        private void OnDisable() => handler.Dispose();

        private void SetWeapon(GameObject prefab)
        {
            foreach (Transform child in weaponRoot)
                Destroy(child.gameObject);

            Instantiate(prefab, weaponRoot);
        }
    }
}