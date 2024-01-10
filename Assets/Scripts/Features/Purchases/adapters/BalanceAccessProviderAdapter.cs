using System;
using Features.Balance.domain;
using Features.Purchases.domain;
using Features.Purchases.domain.model;
using UniRx;
using Zenject;
using static Features.Balance.domain.DecreaseBalanceUseCase;

namespace Features.Purchases.adapters
{
    public class BalanceAccessProviderAdapter : IBalanceAccessProvider
    {
        [Inject] private DecreaseBalanceUseCase decreaseBalanceUseCase;

        public IObservable<bool> CanRemove(CurrencyPurchaseData data) => 
            decreaseBalanceUseCase.GetCanDecreaseFlow(data.Cost, data.CurrencyId);

        public IObservable<bool> Remove(CurrencyPurchaseData data)
        {
            return decreaseBalanceUseCase
                .Decrease(data.Cost, data.CurrencyId)
                .Select(result => result == DecreaseBalanceResult.Success);
        }
    }
}