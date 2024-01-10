using System;
using UniRx;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Shooter.domain
{
    public class SelectWeaponUseCase : IDisposable
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        private readonly CompositeDisposable composite = new();

        public void Select(long weaponId)
        {
            var data = new SelectWeaponRequestData(weaponId);
            Request(data);
        }

        private void Request(SelectWeaponRequestData data) => commandsUseCase
            .Request<Unit, SelectWeaponRequestData>(Commands.IntentSelectWeapon, data)
            .Subscribe()
            .AddTo(composite);

        public void Dispose() => composite.Dispose();

        [Serializable]
        private struct SelectWeaponRequestData
        {
            public long weaponId;

            public SelectWeaponRequestData(long weaponId) : this()
            {
                this.weaponId = weaponId;
            }
        }
    }
}