using Michsky.MUIP;
using UnityEngine;
using UnityEngine.Localization;

namespace Core.Localization.presentation
{
    [RequireComponent(typeof(NotificationManager))]
    public class LocalizeNotification: MonoBehaviour
    {
        [SerializeField, HideInInspector] private NotificationManager manager;
        
        [SerializeField] private LocalizedString title;
        [SerializeField] private LocalizedString description;

        private void Reset() {
            manager = GetComponent<NotificationManager>();
        }

        private void Awake() {
            title.StringChanged += SetTitle;
            description.StringChanged += SetDesc;
        }

        private void OnDestroy() {
            title.StringChanged -= SetTitle;
            description.StringChanged -= SetDesc;
        }

        private void SetDesc(string desc) {
            manager.description = desc;
            manager.UpdateUI();
        }

        private void SetTitle(string title) {
            manager.title = title;
            manager.UpdateUI();
        }
    }
}