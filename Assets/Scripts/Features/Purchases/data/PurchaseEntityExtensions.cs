using Features.Purchases.data.model;
using Features.Purchases.domain.model;

namespace Features.Purchases.data
{
    public static class PurchaseEntityExtensions
    {
        public static PurchaseType GetPurchaseType(this PurchaseEntity entity) => entity.rewardedVideoCount > 0
            ? PurchaseType.RewardedVideo
            : PurchaseType.Currency;
    }
}