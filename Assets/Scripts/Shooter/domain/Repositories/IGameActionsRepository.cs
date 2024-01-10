using System;
using Shooter.domain.Model;

namespace Shooter.domain.Repositories
{
    public interface IGameActionsRepository
    {
        public IObservable<ActionHit> Hits { get; }
        public IObservable<ActionJoinedStateChange> JoinedStateChanges { get; }
        public IObservable<ActionShoot> Shoots { get; }
    }
}