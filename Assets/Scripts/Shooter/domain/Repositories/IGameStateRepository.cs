using System;
using Shooter.domain.Model;

namespace Shooter.domain.Repositories
{
    public interface IGameStateRepository
    {
        public IObservable<GameState> state { get; }
    }
}