using System;
using Michsky.MUIP;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI.Lobby
{
    public class LoadoutScreen : MonoBehaviour
    {
        [Inject] private IWeaponStoreRepository weaponStoreRepository;

        [SerializeField] private ModalWindowManager storeManager;
        [SerializeField] private LoadoutItem primary;
        [SerializeField] private LoadoutItem secondary;

        private void Start()
        {
            primary.SetClickAction(OnClickPrimary);
            secondary.SetClickAction(OnClickSecondary);
            weaponStoreRepository
                .GetWeaponsData()
                .Subscribe(UpdateData)
                .AddTo(this);
        }
        
        private void OnClickSecondary() => storeManager.Open();

        private void OnClickPrimary() => storeManager.Open();

        private void UpdateData(WeaponsData data)
        {
            var primaryId = data.primaryId;
            var secondaryId = data.secondaryId;
            foreach (var weapon in data.weapons)
            {
                var info = weapon.info;
                if (info.id == primaryId) primary.Setup(weapon);
                if (info.id == secondaryId) secondary.Setup(weapon);
            }
        }
    }
}