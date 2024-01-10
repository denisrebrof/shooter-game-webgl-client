using UnityEngine;

namespace Plugins.GoogleAnalytics
{
    public class GoogleAnalyticsEventHandler : MonoBehaviour
    {
        [SerializeField] private string eventName;
        [SerializeField] private string eventArgName;
        [SerializeField] private string eventArgValue;

        public void Send()
        {
            if (string.IsNullOrEmpty(eventName)) return;

            if (string.IsNullOrEmpty(eventArgName))
            {
                GoogleAnalyticsSDK.SendEvent(eventName);
                return;
            }

            if (int.TryParse(eventArgValue, out var numEventArg))
            {
                GoogleAnalyticsSDK.SendNumEvent(eventName, eventArgName, numEventArg);
                return;
            }

            GoogleAnalyticsSDK.SendStringEvent(eventName, eventArgName, eventArgValue);
        }
    }
}