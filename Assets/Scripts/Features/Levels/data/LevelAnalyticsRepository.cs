using System;
using Core.Analytics.levels;
using Plugins.GoogleAnalytics;

namespace Features.Levels.data
{
    public class LevelAnalyticsRepository
    {
        public void SendLevelEvent(long levelId, LevelEvent levelEvent)
        {
#if GAME_ANALYTICS
#elif PLAYFAB_ANALYTICS
#elif GOOGLE_ANALYTICS
            var eventName = "level_" + levelEvent switch
            {
                LevelEvent.Load => "loaded",
                LevelEvent.Start => "started",
                LevelEvent.Fail => "failed",
                LevelEvent.Complete => "complete",
                _ => throw new ArgumentOutOfRangeException(nameof(levelEvent), levelEvent, null)
            } + "_" + levelId;
            GoogleAnalyticsSDK.SendEvent(eventName);
#elif DEBUG_ANALYTICS
            Debug.Log("SendLevelEvent: " + levelId + ' ' + levelEvent);
#endif
        }
    }
}