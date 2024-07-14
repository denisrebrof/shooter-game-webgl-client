using System;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Shooter.domain
{
    public class SetWeaponSlotUseCase
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        public IObservable<bool> TrySetSlot(long weaponId, bool isPrimary)
        {
            var data = new RequestData(weaponId, isPrimary);
            return commandsUseCase.Request<bool, RequestData>(Commands.SetWeaponSlot, data);
        }

        [Serializable]
        private struct RequestData
        {
            public long weaponId;
            public bool primary;

            public RequestData(long weaponId, bool isPrimary)
            {
                this.weaponId = weaponId;
                primary = isPrimary;
            }
        }
    }
}