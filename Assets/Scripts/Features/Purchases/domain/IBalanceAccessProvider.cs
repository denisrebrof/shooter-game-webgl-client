using System;
using Features.Purchases.domain.model;

namespace Features.Purchases.domain
{
    public interface IBalanceAccessProvider
    {
        IObservable<bool> CanRemove(CurrencyPurchaseData data);
        IObservable<bool> Remove(CurrencyPurchaseData data);
    }
}