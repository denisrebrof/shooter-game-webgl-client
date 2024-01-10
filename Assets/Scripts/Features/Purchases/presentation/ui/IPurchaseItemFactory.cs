using Features.Purchases.domain.model;

namespace Features.Purchases.presentation.ui
{
    public interface IPurchaseItemFactory
    {
        PurchaseItem Create(PurchaseType type);
    }
}