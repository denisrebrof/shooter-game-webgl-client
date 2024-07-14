using UnityEngine;
#if !UNITY_EDITOR
using UnityEngine.Localization.Settings;
#endif
using Zenject;

namespace Core.Localization.presentation
{
    public class LocalizationSetup : MonoBehaviour
    {
        [Inject] private ILanguageProvider languageProvider;

        private void Awake() {
#if !UNITY_EDITOR
            var language = languageProvider.GetCurrentLanguage();
            LocalizationSettings.SelectedLocale.Identifier = language == Language.Russian ? "ru" : "en";
#endif
        }
    }
}