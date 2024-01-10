namespace Features.Purchases.data
{
    public interface ISavedPurchasedStateDao
    {
        bool GetPurchasedState(string purchaseId);
        void SetPurchasedState(string purchaseId);
    }
}