using System;
using Features.Purchases.domain.model;
using Features.Purchases.domain.repositories;
using UniRx;
using Zenject;
using static Features.Purchases.domain.CurrencyPurchaseUseCase;
using static Features.Purchases.domain.RewardedVideoPurchaseUseCase;

namespace Features.Purchases.domain
{
    public class ExecutePurchaseUseCase
    {
        [Inject] private IPurchaseAnalyticsRepository analytics;
        [Inject] private PurchasedStateUseCase purchasedStateUseCase;
        [Inject] private CurrencyPurchaseUseCase currencyPurchaseUseCase;
        [Inject] private PurchaseAvailableUseCase purchaseAvailableUseCase;
        [Inject] private RewardedVideoPurchaseUseCase rewardedVideoPurchaseUseCase;
        [Inject] private IPurchaseRepository repository;

        public IObservable<PurchaseResult> ExecutePurchase(string purchaseId) => purchasedStateUseCase
            .GetPurchasedState(purchaseId)
            .Select(purchased => purchased
                ? Observable.Return(PurchaseResult.AlreadyPurchased)
                : TryPurchase(purchaseId)
            ).Switch();

        private IObservable<PurchaseResult> TryPurchase(string purchaseId) => repository.GetById(purchaseId).Type switch
        {
            PurchaseType.Currency => TryCurrencyPurchase(purchaseId),
            PurchaseType.RewardedVideo => TryRewardedVideoPurchase(purchaseId),
            _ => throw new ArgumentOutOfRangeException()
        };

        private IObservable<PurchaseResult> TryRewardedVideoPurchase(string purchaseId) => rewardedVideoPurchaseUseCase
            .LaunchRewardedVideo(purchaseId)
            .Select(result =>
                result == GoVideoPurchaseResult.Purchased ? PurchaseResult.Success : PurchaseResult.Unavailable
            );

        private IObservable<PurchaseResult> TryCurrencyPurchase(string purchaseId) => purchaseAvailableUseCase
            .GetPurchaseAvailable(purchaseId)
            .Take(1)
            .Where(available => available)
            .SelectMany(currencyPurchaseUseCase.ExecutePurchase(purchaseId))
            .Select(res =>
                res switch
                {
                    CurrencyPurchaseResult.Success => PurchaseResult.Success,
                    CurrencyPurchaseResult.AlreadyPurchased => PurchaseResult.AlreadyPurchased,
                    CurrencyPurchaseResult.Failure => PurchaseResult.Unavailable,
                    _ => throw new ArgumentOutOfRangeException(nameof(res), res, null)
                }
            )
            .Do(result =>
            {
                if (result == PurchaseResult.Success) analytics.SendPurchasedEvent(purchaseId);
            });

        public enum PurchaseResult
        {
            Success,
            AlreadyPurchased,
            Unavailable
        }
    }
}