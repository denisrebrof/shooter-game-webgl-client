using System;
using UniRx;
using UnityEngine;

namespace Shooter.presentation
{
    public class PlayerIdProvider: MonoBehaviour
    {
        private readonly BehaviorSubject<long> playerIdSubject = new(-1);

        public void SetPlayerId(long id) => playerIdSubject.OnNext(id);

        public IObservable<long> PlayerIdFlow => playerIdSubject
            .Where(id => id > 0);

        public bool GetPlayerId(out long playerId)
        {
            playerId = playerIdSubject.Value;
            return playerId > 0;
        }
    }
}