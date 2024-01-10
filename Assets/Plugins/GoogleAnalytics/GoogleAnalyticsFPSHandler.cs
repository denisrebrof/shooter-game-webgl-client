using UnityEngine;

namespace Plugins.GoogleAnalytics
{
    public class GoogleAnalyticsFPSHandler : MonoBehaviour
    {
        public void HandleFPS(int fps)
        {
            // Debug.Log("Send FPS: " + fps);
            GoogleAnalyticsSDK.SendNumEvent("measure_fps", "fps", fps);
        }
    
        public void HandleFPSOnce(int fps)
        {
            Debug.Log("Send FPS once: " + fps);
            GoogleAnalyticsSDK.SendNumEvent("measure_fps_once", "fps_once", fps);
        }
    }
}