using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Shooter.presentation.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Shooter.presentation.UI
{
    public class WeaponsPanelController: MonoBehaviour
    {
        [Inject] private WeaponDataSO weaponDataSo;
        
        [SerializeField] private List<Image> weaponIcons;
        [SerializeField] private Color inactiveColor;
        [SerializeField] private int weaponsCount;

        private Image selectedWeapon;

        private void Awake()
        {
            for (long i = 0; i < weaponsCount; i++)
            {
                var icon = weaponIcons[(int)i];
                icon.sprite = weaponDataSo.GetData(i).icon;
                icon.color = inactiveColor;
            }
        }

        public void SetActiveWeapon(int weaponId)
        {
            if(weaponIcons.Count <= weaponId)
                return;

            if (selectedWeapon != null)
                selectedWeapon.color = inactiveColor;

            selectedWeapon = weaponIcons[weaponId];
            selectedWeapon.color = Color.white;
        }
    }
}