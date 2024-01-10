using Core.Localization;
#if VK_SDK
using Plugins.Platforms.VKSDK;
#endif
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Social
{
    [RequireComponent(typeof(Button))]
    public class ShareButton : MonoBehaviour
    {
        [Inject] private ILanguageProvider languageProvider;
        [SerializeField, TextArea(3, 10)] private string ru;
        [SerializeField, TextArea(3, 10)] private string en;
        
        private void Start() => GetComponent<Button>().onClick.AddListener(Share);

        private void Share()
        {
            string local;
            try
            {
                local = GetLocalizedText();
            }
            catch
            {
                local = ru;
            }
#if VK_SDK
            VKSDK.instance.ShowWallPost(local, "https://vk.com/app8042155");
#endif
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
    }
}
