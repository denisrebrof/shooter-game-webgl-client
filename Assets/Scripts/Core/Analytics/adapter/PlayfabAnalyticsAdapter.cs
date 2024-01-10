#if PLAYFAB_ANALYTICS
using System.Collections.Generic;
using Core.Analytics.ads;
using Core.Analytics.screens;
using Core.Analytics.settings;
using Core.SDK.SDKType;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EventsModels;

namespace Core.Analytics.adapter
{
    public class PlayfabAnalyticsAdapter : AnalyticsAdapter
    {
        public override void SendAdEvent(AdAction action, AdType type, AdProvider provider, IAdPlacement placement)
        {
            if (action != AdAction.Show)
                return;

            var request = new ReportAdActivityRequest
            {
                Activity = AdActivity.Start,
                PlacementId = placement.GetName()
            };
            PlayFabClientAPI.ReportAdActivity(request, _ => { }, _ => { });
        }

        public override void SetPlatform(SDKProvider.SDKType platform)
        {
            var updateDataRequest = new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>
                {
                    {"Platform", platform.ToString()}
                }
            };
            PlayFabClientAPI.UpdateUserData(updateDataRequest, _ => { }, _ => { });
        }

        public override void SendSettingsEvent(SettingType type, bool val)
        {
            var updateDataRequest = new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>()
                {
                    {"Settings_" + type, val ? "enabled" : "disabled"}
                }
            };
            PlayFabClientAPI.UpdateUserData(updateDataRequest, _ => { }, _ => { });
            var eventsRequest = new WriteEventsRequest
            {
                Events = new List<EventContents>()
                {
                    new()
                    {
                        EventNamespace = "custom.Settings",
                        Name = "SetSettings",
                        Payload = val
                    }
                }
            };
            PlayFabEventsAPI.WriteEvents(eventsRequest, _ => { }, _ => { });
        }

        public override void SendScreenEvent(string screenName, ScreenAction action)
        {
            var request = new WriteEventsRequest
            {
                Events = new List<EventContents>()
                {
                    new()
                    {
                        EventNamespace = "custom.Navigation",
                        Name = "ScreenEvent",
                        Payload = screenName + ", " + action
                    }
                }
            };
            PlayFabEventsAPI.WriteEvents(request, _ => { }, _ => { });
        }
    }
}
#endif