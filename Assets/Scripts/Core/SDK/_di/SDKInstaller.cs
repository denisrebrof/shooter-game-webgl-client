#if CRAZY_SDK
using CrazyGames;
#elif YANDEX_SDK
using Plugins.Platforms.YSDK;
#elif VK_SDK
using Plugins.Platforms.VKSDK;
#elif POKI_SDK
using Poki.Plugins.Platforms.Poki;
using SDK.HappyTime.controller;
#endif

using Core.SDK.GameState;
using Core.SDK.HappyTime;
using Core.SDK.HappyTime.controller;
using Core.SDK.Platform.domain;
using UnityEngine;
using Zenject;

namespace Core.SDK._di
{
    [CreateAssetMenu(menuName = "Installers/SDKInstaller")]
    public class SDKInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            InstallGameStateNavigator();
            InstallSDK();
            InstallHappyTime();
            InstallPlatformProvider();
        }

        private void InstallHappyTime()
        {
            Container
                .Bind<IHappyTimeController>()
#if POKI_SDK
                .To<PokiHappyTimeController>()
#elif CRAZY_SDK
                .To<CrazyHappyTimeController>()
#else
                .To<DebugLogHappyTimeController>()
#endif
                .AsSingle();
        }

        private void InstallGameStateNavigator() => Container.Bind<GameStateNavigator>().AsSingle();

        private void InstallSDK()
        {
#if YANDEX_SDK
            BindSDK<YandexSDK>("YandexSDK");
#elif VK_SDK
            BindSDK<VKSDK>("VKSDK");
#elif POKI_SDK
            BindSDK<PokiUnitySDK>("POKI_SDK");
#elif CRAZY_SDK
            BindSDK<CrazySDK>("CrazySDK");
#endif
        }

        private void BindSDK<T>(string gameObjectName) where T : Component
        {
            var sdkInstance = new GameObject(gameObjectName);
            var sdkComponent = sdkInstance.AddComponent<T>();
            Container.Bind<T>().FromInstance(sdkComponent).AsSingle();
        }

        private void InstallPlatformProvider()
        {
#if YANDEX_SDK && !UNITY_EDITOR
        Container.Bind<IPlatformProvider>().To<YandexPlatformProvider>().AsSingle();
// #elif UNITY_EDITOR
//         Container.Bind<IPlatformProvider>().To<MobilePlatformProvider>().AsSingle();
#else
            Container.Bind<IPlatformProvider>().To<DesktopPlatformProvider>().AsSingle();
#endif
        }
    }
}