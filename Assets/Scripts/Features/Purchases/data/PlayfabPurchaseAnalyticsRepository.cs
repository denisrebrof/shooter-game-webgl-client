#if PLAYFAB_ANALYTICS
using System.Collections.Generic;
using Features.Purchases.domain.repositories;
using PlayFab;
using PlayFab.EventsModels;

namespace Features.Purchases.data
{
    public class PlayfabPurchaseAnalyticsRepository : IPurchaseAnalyticsRepository
    {
        public void SendPurchasedEvent(string purchaseId)
        {
            PlayFabEventsAPI.WriteEvents(
                new WriteEventsRequest
                {
                    Events = new List<EventContents>()
                    {
                        new()
                        {
                            EventNamespace = "custom.Economy",
                            Name = "Purchase",
                            Payload = "purchaseId: " + purchaseId
                        }
                    }
                },
                _ => { },
                _ => { }
            );
        }
    }
}
#endif