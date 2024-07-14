using System;
using Shooter.domain.Model;
using Shooter.presentation.UI.Lobby;

namespace Shooter.domain.Repositories
{
    public interface ILevelProgressionRepository
    {
        public IObservable<LevelProgressionData> Get();
    }
}