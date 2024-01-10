using Features.Balance.data;
using Features.Balance.domain;
using Features.Balance.domain.repositories;
using Features.Balance.presentation;
using UnityEngine;
using Zenject;

namespace Features.Balance._di
{
    [CreateAssetMenu(menuName = "Installers/BalanceInstaller")]
    public class BalanceInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IBalanceRepository>()
#if PLAYER_PREFS_STORAGE
                .To<PlayerPrefsBalanceRepository>()
#elif ENABLE_WEBSOCKET_CLIENT
                .To<WSBalanceRepository>()
#else
                .To<LocalStorageBalanceRepository>()
                // .To<InfiniteThousandBalanceRepository>()
#endif
                .AsSingle();

            Container.Bind<IRewardRepository>().To<RewardInMemoryRepository>().AsSingle();
            Container.Bind<DecreaseBalanceUseCase>().AsSingle();
            Container.Bind<CollectRewardUseCase>().AsSingle();

            var addBalanceNavigator = FindObjectOfType<AddBalanceNavigator>();
            Container.BindInstance(addBalanceNavigator).AsSingle();
        }
    }
}