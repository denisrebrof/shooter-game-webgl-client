using System;
using UniRx;
using UnityEngine;

namespace Shooter.presentation.Player.Weapons
{
    public class SelectedWeaponTypeAnimationController : PlayerDataBehaviour
    {
        private static readonly int Armed = Animator.StringToHash("Armed");
        [SerializeField] private Animator target;

        private IDisposable handler = Disposable.Empty;

        private void OnEnable()
        {
            handler = PlayerDataFlow
                .Select(data => data.selectedWeaponId)
                .DistinctUntilChanged()
                .Select(id => id > 0)
                .Subscribe(SetWeaponArmed)
                .AddTo(this);
        }

        private void OnDisable()
        {
            handler.Dispose();
        }

        private void SetWeaponArmed(bool armed)
        {
            target.SetBool(Armed, armed);
        }
    }
}