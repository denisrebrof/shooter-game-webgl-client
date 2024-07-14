using System;
using Shooter.data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Player.Weapons
{
    public class PlayerWeaponsController : PlayerDataBehaviour
    {
        [SerializeField] private CharacterView characterView;

        private IDisposable handler = Disposable.Empty;

        [Inject] private WeaponDataSO weaponData;

        private void OnEnable()
        {
            // handler = PlayerDataFlow
            //     .Select(data => data.selectedWeaponId)
            //     .DistinctUntilChanged()
            //     .Select(id => weaponData.GetData(id).prefab)
            //     .Subscribe(SetWeapon)
            //     .AddTo(this);
        }

        private void OnDisable()
        {
            handler.Dispose();
        }

        private void SetWeapon(GameObject prefab)
        {
            characterView.SetWeapon(prefab);
        }
    }
}