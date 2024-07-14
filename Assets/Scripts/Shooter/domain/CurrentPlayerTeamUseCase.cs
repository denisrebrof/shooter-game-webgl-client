using System;
using Shooter.domain.Model;
using UniRx;
using Zenject;

namespace Shooter.domain
{
    public class CurrentPlayerTeamUseCase
    {
        [Inject] private CurrentPlayerStateUseCase playerStateUseCase;
        
        public IObservable<Teams> TeamFlow => playerStateUseCase
            .GetStateFlow()
            .First()
            .Select(data => data.Team);
    }
}