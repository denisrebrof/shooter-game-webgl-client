﻿using System;
using UniRx;
using UnityEngine;

namespace Shooter.presentation
{
    public class PlayerIdProvider : MonoBehaviour
    {
        private const long DefaultPlayerId = long.MaxValue;
        [SerializeField] private long visibleId = DefaultPlayerId;

        private readonly BehaviorSubject<long> playerIdSubject = new(DefaultPlayerId);

        public IObservable<long> PlayerIdFlow => playerIdSubject
            .Where(id => id != DefaultPlayerId);

        public void SetPlayerId(long id)
        {
            visibleId = id;
            playerIdSubject.OnNext(id);
        }

        public bool GetPlayerId(out long playerId)
        {
            playerId = playerIdSubject.Value;
            return playerId != DefaultPlayerId;
        }
    }
}