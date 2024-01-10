using Features.Purchases.adapters;
using Features.Purchases.data;
using Features.Purchases.data.dao;
using Features.Purchases.domain;
using Features.Purchases.domain.repositories;
using Features.Purchases.presentation.ui;
using UnityEngine;
using Zenject;

namespace Features.Purchases._di
{
    [CreateAssetMenu(fileName = "PurchasesBaseInstaller", menuName = "Installers/PurchasesBaseInstaller")]
    public class PurchasesBaseInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            //Daos
            BindPurchasedStateDao();
            BindRewardedVideoWatchDao();
            Container.Bind<PurchaseEntityConverter>().ToSelf().AsSingle();
            //Repositories
            Container.BindInterfacesAndSelfTo<DefaultPurchaseRepository>().AsSingle();
            Container.Bind<ICurrencyPurchaseRepository>().To<CurrencyPurchaseRepository>().AsSingle();
            Container.Bind<IRewardedVideoPurchaseRepository>().To<RewardedVideoPurchaseRepository>().AsSingle();
            Container.Bind<IPurchaseAnalyticsRepository>()
#if PLAYFAB_ANALYTICS
                .To<PlayfabPurchaseAnalyticsRepository>()
#else
                .To<DebugLogAnalyticsPurchaseRepository>()
#endif
                .AsSingle();
            //UseCases
            Container.Bind<CurrencyPurchaseUseCase>().ToSelf().AsSingle();
            Container.Bind<PurchasedStateUseCase>().ToSelf().AsSingle();
            Container.Bind<PurchaseAvailableUseCase>().ToSelf().AsSingle();
            Container.Bind<RewardedVideoPurchaseUseCase>().ToSelf().AsSingle();
            Container.Bind<ExecutePurchaseUseCase>().ToSelf().AsSingle();
            //Adapters
            Container
                .Bind<IBalanceAccessProvider>()
                .To<BalanceAccessProviderAdapter>()
                .AsSingle();
            Container
                .Bind<DefaultPurchaseItemController.IPurchaseItemSelectionAdapter>()
                .To<PurchaseItemSelectionAdapter>()
                .AsSingle();
        }

        private void BindRewardedVideoWatchDao()
        {
            Container
                .Bind<RewardedVideoPurchaseRepository.IRewardedVideoWatchDao>()
#if PLAYER_PREFS_STORAGE
                .To<PlayerPrefsRewardedVideoWatchesDao>()
#else
                .To<LocalStorageRewardedVideoWatchesDao>()
#endif
                .AsSingle();
        }

        private void BindPurchasedStateDao()
        {
            Container
                .Bind<ISavedPurchasedStateDao>()
#if PLAYER_PREFS_STORAGE
                .To<PlayerPrefsPurchasedStateDao>()
#else
                .To<LocalStoragePurchasedStateDao>()
#endif
                .AsSingle();
        }
    }
}