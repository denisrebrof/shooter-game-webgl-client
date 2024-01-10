using Features.Purchases.adapters;
using Features.Purchases.domain;
using Features.Purchases.presentation.ui;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Features.Purchases._di
{
    public class PurchasesPresentationInstaller : MonoInstaller
    {
        [FormerlySerializedAs("coinsPurchaseItemPrefab")] [SerializeField] private CurrencyPurchaseItem currencyPurchaseItemPrefab;
        [SerializeField] private RewardedVideoPurchaseItem rewardedVideoPurchaseItemPrefab;

        public override void InstallBindings()
        {
            //Presentation
            //UI
            Container
                .Bind<PurchaseItem.IPurchaseItemController>()
                .To<DefaultPurchaseItemController>()
                .FromNewComponentOnNewGameObject().AsTransient();
            //Item Factories
            Container
                .BindFactory<CurrencyPurchaseItem, CurrencyPurchaseItem.Factory>()
                .FromComponentInNewPrefab(currencyPurchaseItemPrefab);
            Container
                .BindFactory<RewardedVideoPurchaseItem, RewardedVideoPurchaseItem.Factory>()
                .FromComponentInNewPrefab(rewardedVideoPurchaseItemPrefab);
            Container
                .Bind<IPurchaseItemFactory>()
                .To<DefaultPurchaseItemFactory>()
                .AsSingle();
        }
    }
}