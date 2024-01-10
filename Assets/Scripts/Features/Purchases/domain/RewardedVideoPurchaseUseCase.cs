using System;
using Core.RewardedVideo.domain;
using Core.RewardedVideo.domain.model;
using Features.Purchases.domain.repositories;
using UniRx;
using Zenject;

namespace Features.Purchases.domain
{
    public class RewardedVideoPurchaseUseCase
    {
        [Inject] private IRewardedVideoPurchaseRepository rewardedVideoPurchaseRepository;
        [Inject] private PurchasedStateUseCase purchasedStateUseCase;
        [Inject] private IRewardedVideoNavigator rewardedVideoNavigator;

        public IObservable<GoVideoPurchaseResult> LaunchRewardedVideo(string purchaseId) =>
            rewardedVideoNavigator
                .ShowRewardedVideo()
                .Select(result => result == ShowRewardedVideoResult.Success)
                .SelectMany(result =>
                    GetPurchaseShowVideoResult(result, purchaseId)
                );

        private IObservable<GoVideoPurchaseResult> GetPurchaseShowVideoResult(bool videoShown,
            string purchaseId)
        {
            if (!videoShown)
                return Observable.Return(GoVideoPurchaseResult.Failure);

            rewardedVideoPurchaseRepository.AddRewardedVideoWatch(purchaseId);
            return purchasedStateUseCase
                .GetPurchasedState(purchaseId)
                .Select(purchased =>
                    purchased ? GoVideoPurchaseResult.Purchased : GoVideoPurchaseResult.InProgress
                );
        }

        public enum GoVideoPurchaseResult
        {
            Purchased,
            InProgress,
            Failure
        }
    }
}