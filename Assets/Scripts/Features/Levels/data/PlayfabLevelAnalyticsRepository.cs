#if PLAYFAB_ANALYTICS
using System.Collections.Generic;
using Core.Analytics.levels;
using Features.Levels.domain;
using PlayFab;
using PlayFab.EventsModels;
using UnityEngine;

namespace Features.Levels.data
{
    public class PlayfabLevelAnalyticsRepository: ILevelsAnalyticsRepository
    {
        public void SendLevelEvent(long levelId, LevelEvent levelEvent)
        {
            Debug.Log("Analytics SendLevelEvent");
            var request = new WriteEventsRequest
            {
                Events = new List<EventContents>()
                {
                    new()
                    {
                        EventNamespace = "custom.Levels",
                        Name = levelEvent.ToString(),
                        Payload = "level: " + levelId
                    }
                }
            };
            PlayFabEventsAPI.WriteEvents(
                request,
                _ => { Debug.Log("Analytics SendLevelEvent res"); },
                _ => { Debug.Log("Analytics SendLevelEvent err"); }
            );
        }
    }
}
#endif