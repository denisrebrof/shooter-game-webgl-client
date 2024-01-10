using System;
using Features.Purchases.domain.model;
using Features.Purchases.domain.repositories;
using UniRx;
using Zenject;

namespace Features.Purchases.domain
{
    public class PurchasedStateUseCase
    {
        [Inject] private IPurchaseRepository repository;
        [Inject] private ICurrencyPurchaseRepository currencyPurchaseRepository;
        [Inject] private IRewardedVideoPurchaseRepository videoPurchaseRepository;

        public IObservable<Boolean> GetPurchasedState(string purchaseId)
        {
            var type = repository.GetById(purchaseId).Type;
            switch (type)
            {
                case PurchaseType.Currency:
                    return currencyPurchaseRepository.GetPurchasedState(purchaseId);
                case PurchaseType.RewardedVideo:
                    var currentWatchesFlow = videoPurchaseRepository.GetRewardedVideoCurrentWatchesCount(purchaseId);
                    var requiredWatches = videoPurchaseRepository.GetRewardedVideoWatchesCount(purchaseId);
                    return currentWatchesFlow.Select(currentWatches => currentWatches >= requiredWatches);
                default:
                    return Observable.Return(false);
            }
        }
    }
}