using System;
using System.Collections;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace Shooter.presentation.UI
{
    public class PlayerHp : PlayerDataBehaviour
    {
        [SerializeField] private Transform hpRoot;
        [SerializeField] private Transform hpBar;

        [CanBeNull] private IDisposable handler;

        [CanBeNull] private Transform lookAtTarget;

        private void OnEnable()
        {
            StartCoroutine(GetCamCoroutine());
            handler = PlayerDataFlow
                .Select(state => state.hp)
                .Subscribe(hp => hpBar.localScale = new Vector3(hp / 100f, 1f, 1f));
        }

        private IEnumerator GetCamCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
            lookAtTarget = GameObject.FindWithTag("PlayerCamera")?.transform;
        }

        private void Update()
        {
            if (lookAtTarget == null)
                return;

            hpRoot.LookAt(lookAtTarget);
        }

        private void OnDisable()
        {
            handler?.Dispose();
            StopAllCoroutines();
        }
    }
}