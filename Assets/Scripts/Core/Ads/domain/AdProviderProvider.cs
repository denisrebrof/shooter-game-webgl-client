using Core.SDK.SDKType;

namespace Core.Ads.domain
{
    public static class AdProviderProvider
    {
        public static AdProvider CurrentProvider => SDKProvider.GetSDK() switch
        {
            SDKProvider.SDKType.Yandex => AdProvider.Yandex,
            SDKProvider.SDKType.Vk => AdProvider.VK,
            SDKProvider.SDKType.Poki => AdProvider.Poki,
            _ => AdProvider.None
        };
    }
}