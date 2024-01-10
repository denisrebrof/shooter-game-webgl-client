#if UNITY_WEBGL && !UNITY_EDITOR && GOOGLE_ANALYTICS
using System.Runtime.InteropServices;
#endif

namespace Plugins.GoogleAnalytics
{
    public static class GoogleAnalyticsSDK
    {
        public static void SendEvent(string eventName)
        {
#if UNITY_WEBGL && !UNITY_EDITOR && GOOGLE_ANALYTICS
        Send(eventName);
#endif
        }
    
        public static void SendNumEvent(string eventName, string argName, int argValue)
        {
#if UNITY_WEBGL && !UNITY_EDITOR && GOOGLE_ANALYTICS
        SendNumArg(eventName, argName, argValue.ToString());
#endif
        }
    
        public static void SendStringEvent(string eventName, string argName, string argValue)
        {
#if UNITY_WEBGL && !UNITY_EDITOR && GOOGLE_ANALYTICS
        SendStringArg(eventName, argName, argValue);
#endif
        }

#if UNITY_WEBGL && !UNITY_EDITOR && GOOGLE_ANALYTICS
    [DllImport("__Internal")]
    private static extern void Send(string eventName);
    [DllImport("__Internal")]
    private static extern void SendNumArg(string eventName, string argName, string argValue);
    [DllImport("__Internal")]
    private static extern void SendStringArg(string eventName, string argName, string argValue);
#endif
    }
}