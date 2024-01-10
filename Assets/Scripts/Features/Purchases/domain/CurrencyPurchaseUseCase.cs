using System;
using Features.Purchases.domain.model;
using Features.Purchases.domain.repositories;
using UniRx;
using Zenject;

namespace Features.Purchases.domain
{
    public class CurrencyPurchaseUseCase
    {
        [Inject] private IPurchaseRepository purchaseRepository;
        [Inject] private ICurrencyPurchaseRepository currencyPurchaseRepository;
        [Inject] private IBalanceAccessProvider balanceAccessProvider;

        public IObservable<CurrencyPurchaseResult> ExecutePurchase(string purchaseId) => currencyPurchaseRepository
            .GetPurchasedState(purchaseId)
            .Take(1)
            .SelectMany(state =>
                state ? Observable.Return(CurrencyPurchaseResult.AlreadyPurchased) : ExecuteNewPurchase(purchaseId)
            );

        private IObservable<CurrencyPurchaseResult> ExecuteNewPurchase(string purchaseId)
        {
            var data = currencyPurchaseRepository.GetData(purchaseId);
            var type = purchaseRepository.GetById(purchaseId).Type;
            if (type != PurchaseType.Currency)
                return Observable.Return(CurrencyPurchaseResult.Failure);
            
            return balanceAccessProvider
                .CanRemove(data)
                .Take(1)
                .SelectMany(enoughBalance =>
                    {
                        if (!enoughBalance) return Observable.Return(CurrencyPurchaseResult.Failure);
                        return balanceAccessProvider.Remove(data).Select(result =>
                            {
                                if (!result)
                                    return CurrencyPurchaseResult.Failure;

                                currencyPurchaseRepository.SetPurchased(purchaseId);
                                return CurrencyPurchaseResult.Success;
                            }
                        );
                    }
                );
        }

        public enum CurrencyPurchaseResult
        {
            Success,
            AlreadyPurchased,
            Failure
        }
    }
}