using System;
using UnityEngine;

namespace Core.Localization.presentation
{
    [Serializable]
    public class SimpleLocalizedTextProvider
    {
        [SerializeField] private string ruText;
        [SerializeField] private string enText;

        public string GetText(ILanguageProvider languageProvider)
        {
            var language = languageProvider.GetCurrentLanguage();
            return language == Language.Russian ? ruText : enText;
        }
    }
}