using TMPro;
using UnityEditor;
using UnityEngine;
using Zenject;
#if UNITY_EDITOR
#endif

namespace Core.Localization.presentation
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedTMP : MonoBehaviour
    {
        [Inject] private ILanguageProvider languageProvider;
        [SerializeField] private int customFontSizeRu = 0;
        [SerializeField] private int customFontSizeEn = 0;
        [SerializeField, TextArea(3, 10)] private string ru;
        [SerializeField, TextArea(3, 10)] private string en;

        private void Start()
        {
            var text = GetComponent<TMP_Text>();
            string local;
            try
            {
                local = GetLocalizedText();
            }
            catch
            {
                local = en;
            }

            text.text = local;
            var size = GetLocalizedSize();
            if (size != 0)
            {
                text.fontSize = size;
            }
        }

        private int GetLocalizedSize()
        {
            var lang = languageProvider.GetCurrentLanguage();
            return lang switch
            {
                Language.Russian => customFontSizeRu,
                Language.English => customFontSizeEn,
                _ => customFontSizeEn
            };
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

#if UNITY_EDITOR
        [ContextMenu("Apply English")]
        private void ApplyEn()
        {
            var text = GetComponent<TMP_Text>();
            text.text = en;
            if (customFontSizeEn > 0)
                text.fontSize = customFontSizeEn;
            EditorApplication.MarkSceneDirty();
        }

        [ContextMenu("Apply Russian")]
        private void ApplyRu()
        {
            var text = GetComponent<TMP_Text>();
            text.text = ru;
            if (customFontSizeRu > 0)
                text.fontSize = customFontSizeRu;
            EditorApplication.MarkSceneDirty();
        }
#endif
    }
}