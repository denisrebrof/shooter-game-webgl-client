using System;
using UniRx;
using UnityEngine;

namespace Shooter.presentation
{
    public class PlayerIdProvider : MonoBehaviour
    {
        private const long DefaultPlayerId = long.MaxValue;

        private readonly BehaviorSubject<long> playerIdSubject = new(DefaultPlayerId);

        public void SetPlayerId(long id) => playerIdSubject.OnNext(id);

        public IObservable<long> PlayerIdFlow => playerIdSubject
            .Where(id => id != DefaultPlayerId);

        public bool GetPlayerId(out long playerId)
        {
            playerId = playerIdSubject.Value;
            return playerId != DefaultPlayerId;
        }
    }
}