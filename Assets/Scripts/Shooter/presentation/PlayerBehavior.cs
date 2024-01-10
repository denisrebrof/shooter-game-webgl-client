using System;
using UnityEngine;

namespace Shooter.presentation
{
    public class PlayerBehavior : MonoBehaviour
    {
        private PlayerIdProvider provider;

        private PlayerIdProvider Provider
        {
            get
            {
                if (provider != null)
                    return provider;

                provider = GetComponentInParent<PlayerIdProvider>();
                return provider;
            }
        }

        protected IObservable<long> PlayerIdFlow => Provider.PlayerIdFlow;
        
        protected bool GetPlayerId(out long playerId) => Provider.GetPlayerId(out playerId);
    }
}