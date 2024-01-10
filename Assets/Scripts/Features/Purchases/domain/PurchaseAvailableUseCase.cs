using System;
using Features.Purchases.domain.model;
using Features.Purchases.domain.repositories;
using UniRx;
using Zenject;

namespace Features.Purchases.domain
{
    public class PurchaseAvailableUseCase
    {
        [Inject] private PurchasedStateUseCase purchasedStateUseCase;
        [Inject] private IPurchaseRepository repository;
        [Inject] private IBalanceAccessProvider balance;
        [Inject] private ICurrencyPurchaseRepository currencyPurchaseRepository;
        [Inject] private IRewardedVideoPurchaseRepository videoPurchaseRepository;

        public IObservable<bool> GetPurchaseAvailable(string purchaseId) => purchasedStateUseCase
            .GetPurchasedState(purchaseId)
            .SelectMany(state =>
                state ? Observable.Return(false) : GetPurchaseAvailableState(purchaseId)
            );

        private IObservable<bool> GetPurchaseAvailableState(string purchaseId)
        {
            var purchase = repository.GetById(purchaseId);
            switch (purchase.Type)
            {
                case PurchaseType.Currency:
                    var data = currencyPurchaseRepository.GetData(purchaseId);
                    return balance.CanRemove(data);
                case PurchaseType.RewardedVideo:
                    var currentWatchesFlow = videoPurchaseRepository.GetRewardedVideoCurrentWatchesCount(purchaseId);
                    var requiredWatches = videoPurchaseRepository.GetRewardedVideoWatchesCount(purchaseId);
                    return currentWatchesFlow.Select(currentWatches => currentWatches + 1 >= requiredWatches);
                default:
                    return Observable.Return(false);
            }
        }
    }
}