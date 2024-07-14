using System;
using Shooter.domain;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Scene
{
    public class FlagController : MonoBehaviour
    {
        [Inject] private CurrentPlayerTeamUseCase currentPlayerTeamUseCase;
        [Inject] private FlagActionsUseCase flagActionsUseCase;

        public bool isEnemyFlag;
        [SerializeField] private float actionRequestDelay = 0.1f;

        [SerializeField, HideInInspector] private GameObject target;
        [SerializeField, HideInInspector] private Transform targetTransform;

        private Transform origin;

        private IDisposable flagActionHandler = Disposable.Empty;

        private void Reset() {
            target = gameObject;
            targetTransform = transform;
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player") || origin != null)
                return;

            flagActionHandler.Dispose();
            flagActionHandler = Observable
                .Timer(TimeSpan.FromSeconds(actionRequestDelay))
                .Repeat()
                .Select(_ => isEnemyFlag ? flagActionsUseCase.TakeFlag() : flagActionsUseCase.ReturnFlag())
                .Switch()
                .Subscribe();
            Debug.Log("Player Enter");
        }

        private void OnTriggerExit(Collider other) {
            if (!other.CompareTag("Player"))
                return;

            flagActionHandler.Dispose();
            Debug.Log("Player Exit");
        }

        public void Activate(bool active) {
            target.SetActive(active);
            if (!active) flagActionHandler.Dispose();
        }

        public void SetOrigin(Transform origin) => this.origin = origin;

        public void SetPosition(Vector3 pos) {
            origin = null;
            targetTransform.position = pos;
        }

        private void Update() {
            if (origin == null)
                return;

            targetTransform.position = origin.position;
        }

        private void OnDestroy() {
            flagActionHandler.Dispose();
        }
    }
}