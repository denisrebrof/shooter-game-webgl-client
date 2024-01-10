using Core.Analytics.adapter;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Analytics.settings
{
    [RequireComponent(typeof(Toggle))]
    public class SettingsToggleAnalyticsEvent : MonoBehaviour
    {
        [Inject] private AnalyticsAdapter analytics;
        [SerializeField] private SettingType type = SettingType.SoundToggle;
        
        private void Start() => GetComponent<Toggle>().onValueChanged.AddListener(Toggle);
        
        //TODO replace with string/object state
        private void Toggle(bool state) => analytics.SendSettingsEvent(type, state);
    }
}