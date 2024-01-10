using System;
using Features.Balance.domain;
using Features.Purchases.domain.model;

namespace Features.Purchases.domain.repositories
{
    public interface ICurrencyPurchaseRepository
    {
        CurrencyPurchaseData GetData(string purchaseId);
        void SetPurchased(string purchaseId);
        IObservable<bool> GetPurchasedState(string purchaseId);
    }
}