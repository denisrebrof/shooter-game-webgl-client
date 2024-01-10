using Core.Ads.domain;
using ModestTree;
using Plugins.GoogleAnalytics;

namespace Core.Ads.data
{
    public class AdAnalyticsRepository
    {
        public void SendAdEvent(AdAction action, AdType type, AdProvider provider, IAdPlacement placement)
        {
#if GAME_ANALYTICS
                
#elif PLAYFAB_ANALYTICS
        
#elif GOOGLE_ANALYTICS
            var eventName = action switch
            {
                AdAction.Request => "ad_request",
                AdAction.Show => "ad_show",
                AdAction.Failure => "ad_failure",
                _ => ""
            };
            if(eventName.IsEmpty())
                return;
            
            GoogleAnalyticsSDK.SendEvent(eventName);
#elif DEBUG_ANALYTICS
            Debug.Log("SendAdEvent: " + action + ' ' + type+ ' ' + provider + ' ' + placement.GetName());
#endif
        }
    }
}