using Core.Localization;
using Features.Hints.domain;
using UnityEngine;
using Utils.PlayerTrigger;
using Zenject;

namespace Features.Hints.presentation
{
    public class Hint : PlayerTriggerBase
    {
        [Inject] private ICurrentHintRepository repository;
        [Inject] private ILanguageProvider languageProvider;
        [Inject] private HintsNavigator hintsNavigator;
        [SerializeField,TextArea(3,10)] private string ru;
        [SerializeField,TextArea(3,10)] private string en;

        public void Setup()
        {
            string text;
            try
            {
                text = GetLocalizedText();
            }
            catch
            {
                text = ru;
            }

            repository.SetHintText(text);
        }

        private string GetLocalizedText()
        {
            var lang = languageProvider.GetCurrentLanguage();
            return lang switch
            {
                Language.Russian => ru,
                Language.English => en,
                _ => en
            };
        }

        protected override void OnPlayerEntersTrigger()
        {
            Setup();
            hintsNavigator.Show();
        }

        protected override void OnPlayerExitTrigger()
        {
            hintsNavigator.Hide();
        }
    }
}