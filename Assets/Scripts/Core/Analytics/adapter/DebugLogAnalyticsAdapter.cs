using Core.Analytics.screens;
using Core.Analytics.settings;
using Core.SDK.SDKType;
using UnityEngine;

namespace Core.Analytics.adapter
{
    public class DebugLogAnalyticsAdapter : AnalyticsAdapter
    {
        private readonly bool loggingEnabled = false;

        public DebugLogAnalyticsAdapter(bool loggingEnabled)
        {
            this.loggingEnabled = loggingEnabled;
        }

        public override void SetPlatform(SDKProvider.SDKType platform) => Log(" SetPlatform: " + platform);

        public override void SendSettingsEvent(SettingType type, bool val) =>
            Log("SendSettingsEvent: " + type + ' ' + val);

        public override void SendScreenEvent(string screenName, ScreenAction action) =>
            Log("SendScreenEvent: " + screenName + ' ' + action);

        private void Log(string text)
        {
            if (!loggingEnabled) return;
            Debug.Log("Debug Analytics Event: " + text);
        }
    }
}