#if YANDEX_SDK
using Plugins.Platforms.YSDK;
using Zenject;
#endif
namespace Core.SDK.Platform.domain
{
    public class YandexPlatformProvider: IPlatformProvider
    {
#if YANDEX_SDK
        [Inject] private YandexSDK sdk; 
#endif
        public Platform GetCurrentPlatform()
        {
#if YANDEX_SDK
            return sdk.GetIsOnDesktop() ? Platform.Desktop : Platform.Mobile;
#endif
            return Platform.Desktop;
        }
    }
}