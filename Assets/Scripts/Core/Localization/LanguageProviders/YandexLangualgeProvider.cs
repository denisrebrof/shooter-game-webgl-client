#if YANDEX_SDK
using Plugins.Platforms.YSDK;
using Zenject;
#endif

namespace Core.Localization.LanguageProviders
{
    public class YandexLanguageProvider : ILanguageProvider
    {
#if YANDEX_SDK
        [Inject] private YandexSDK sdk;
#endif
        private Language defaultLanguage = Language.English;

        public Language GetCurrentLanguage()
        {
#if YANDEX_SDK && !UNITY_EDITOR
            var lang = sdk.GetLanguage();
            return lang switch
            {
                "ru" => Language.Russian,
                "en" => Language.English,
                _ => defaultLanguage
            };
#endif
            return defaultLanguage;
        }
    }
}