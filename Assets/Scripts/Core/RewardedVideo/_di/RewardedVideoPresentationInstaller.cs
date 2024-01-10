using Core.RewardedVideo.domain;
using Core.RewardedVideo.presentation;
using UnityEngine;
using Zenject;

namespace Core.RewardedVideo._di
{
    public class RewardedVideoPresentationInstaller: MonoInstaller
    {
        [SerializeField] private YandexRewardedVideoNavigator navigator;
                
            public override void InstallBindings()
            {
                //Presenters
                Container
                    .Bind<IRewardedVideoNavigator>()
                    .To<StubRewardedVideoNavigator>()
                    .AsSingle();
            }
    }
}