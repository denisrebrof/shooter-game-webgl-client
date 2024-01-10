using System;
using Core.RewardedVideo.domain.model;

namespace Core.RewardedVideo.domain
{
    public interface IRewardedVideoNavigator
    {
        IObservable<ShowRewardedVideoResult> ShowRewardedVideo();
    }
}