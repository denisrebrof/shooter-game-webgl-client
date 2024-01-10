using Core.Ads.data;
using Core.Ads.presentation;
using Core.Ads.presentation.InterstitialAdNavigator;
using Core.Ads.presentation.InterstitialAdNavigator.core;
using Core.Ads.presentation.InterstitialAdNavigator.decorators;
using UnityEngine;
using Zenject;

namespace Core.Ads._di
{
    public class AdsInstaller : MonoInstaller
    {
        [SerializeField] private InterstitialAdNavigatorMuteAudioDecorator muteAudioInterstitialAdNavigatorDecorator;

        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<AdAnalyticsRepository>()
                .AsSingle();

            Container
                .Bind<IInterstitialAdNavigator>()
#if YANDEX_SDK
                .To<YandexInterstitialAdNavigator>()
                .AsSingle()
                .WhenInjectedInto<YandexInterstitialNavigatorHitsDecorator>();
                
            Container
                .Bind<IInterstitialAdNavigator>()
                .To<YandexInterstitialNavigatorHitsDecorator>()
#elif VK_SDK
                .To<VKInterstitialAdNavigator>()
#elif POKI_SDK
                .To<PokiInterstitialAdNavigator>()
#elif CRAZY_SDK
                .To<CrazyInterstitialAdNavigator>()
#else
                .To<DebugLogInterstitialAdNavigator>()
#endif
                .AsSingle()
                .WhenInjectedInto<InterstitialAdNavigatorAnalyticsDecorator>();

            Container
                .Bind<IInterstitialAdNavigator>()
                .To<InterstitialAdNavigatorAnalyticsDecorator>()
                .FromNew()
                .AsSingle()
                .WhenInjectedInto<InterstitialAdNavigatorLockLookDecorator>();

            Container
                .Bind<IInterstitialAdNavigator>()
                .To<InterstitialAdNavigatorLockLookDecorator>()
                .FromNew()
                .AsSingle()
                .WhenInjectedInto<InterstitialAdNavigatorMuteAudioDecorator>();

            Container
                .Bind<IInterstitialAdNavigator>()
                .To<InterstitialAdNavigatorMuteAudioDecorator>()
                .FromInstance(muteAudioInterstitialAdNavigatorDecorator)
                .AsSingle()
                .WhenInjectedInto<InterstitialAdNavigatorCounterDecorator>();

            Container
                .Bind<IInterstitialAdNavigator>()
                .WithId(IInterstitialAdNavigator.DefaultInstance)
                .To<InterstitialAdNavigatorCounterDecorator>()
                .AsSingle();
        }
    }
}