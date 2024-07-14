using System;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Shooter.domain
{
    public class WeaponStoreOperationsUseCase
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        public IObservable<bool> TryPurchase(long weaponId)
        {
            var data = RequestData.Purchase(weaponId);
            return commandsUseCase.Request<bool, RequestData>(Commands.PurchaseWeapon, data);
        }
        
        public IObservable<bool> TryUpgrade(long weaponId)
        {
            var data = RequestData.Upgrade(weaponId);
            return commandsUseCase.Request<bool, RequestData>(Commands.PurchaseWeapon, data);
        }

        [Serializable]
        private struct RequestData
        {
            public long weaponId;
            public bool isUpgrade;

            public static RequestData Purchase(long weaponId) =>
                new RequestData { weaponId = weaponId, isUpgrade = false };
            
            public static RequestData Upgrade(long weaponId) =>
                new RequestData { weaponId = weaponId, isUpgrade = true };
        }
    }
}