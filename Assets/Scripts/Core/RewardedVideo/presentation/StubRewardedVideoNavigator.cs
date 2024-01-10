using System;
using Core.RewardedVideo.domain;
using Core.RewardedVideo.domain.model;
using UniRx;

namespace Core.RewardedVideo.presentation
{
    public class StubRewardedVideoNavigator: IRewardedVideoNavigator
    {
        public IObservable<ShowRewardedVideoResult> ShowRewardedVideo()
        {
            return Observable.Return(ShowRewardedVideoResult.Success);
        }
    }
}