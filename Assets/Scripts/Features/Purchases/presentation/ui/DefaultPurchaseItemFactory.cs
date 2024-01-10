using System;
using Features.Purchases.domain.model;
using Zenject;

namespace Features.Purchases.presentation.ui
{
    public class DefaultPurchaseItemFactory : IPurchaseItemFactory
    {
        [Inject] private CurrencyPurchaseItem.Factory coinsPurchaseItemFactory;
        [Inject] private RewardedVideoPurchaseItem.Factory rewardedVideoItemFactory;

        public PurchaseItem Create(PurchaseType type) => type switch
        {
            PurchaseType.Currency => coinsPurchaseItemFactory.Create(),
            PurchaseType.RewardedVideo => rewardedVideoItemFactory.Create(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}