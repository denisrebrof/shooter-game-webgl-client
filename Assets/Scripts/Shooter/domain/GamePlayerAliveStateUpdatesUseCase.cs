using System;
using Shooter.domain.Model;
using UniRx;
using Zenject;

namespace Shooter.domain
{
    public class GamePlayerAliveStateUpdatesUseCase
    {
        [Inject] private GamePlayerUseCase playerUseCase;

        public IObservable<AliveStateUpdate> GetAliveStateUpdatesFlow(long playerId) => playerUseCase
            .GetPlayerState(playerId)
            .Pairwise()
            .Where(HasAliveStateUpdate)
            .Select(pair => pair.Current)
            .Select(GetUpdate);

        private static AliveStateUpdate GetUpdate(PlayerData data)
        {
            return new AliveStateUpdate(data.alive, data.pos);
        }

        private static bool HasAliveStateUpdate(Pair<PlayerData> prevToNextData)
        {
            return prevToNextData.Previous.alive != prevToNextData.Current.alive;
        }
    }

    public struct AliveStateUpdate
    {
        public bool Alive;
        public TransformSnapshot Position;

        public AliveStateUpdate(bool alive, TransformSnapshot position)
        {
            Alive = alive;
            Position = position;
        }
    }
}