using System;
using System.Collections.Generic;
using UniRx;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Shooter.domain
{
    public class BotsVisibilityMaskRepository
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        public IObservable<Unit> Submit(
            int playersHash,
            Dictionary<long, long> visibilityPairs
        )
        {
            var targetPairsList = new List<long>();
            foreach (var pair in visibilityPairs)
            {
                targetPairsList.Add(pair.Key);
                targetPairsList.Add(pair.Value);
            }

            var dataPairs = targetPairsList.ToArray();
            var data = new BotsVisibilityData(playersHash, dataPairs);
            return commandsUseCase
                .Request<int, BotsVisibilityData>(Commands.SubmitVisibility, data)
                .AsUnitObservable();
        }

        [Serializable]
        private struct BotsVisibilityData
        {
            public int playersHash;
            public long[] targetPairs;

            public BotsVisibilityData(int playersHash, long[] targetPairs)
            {
                this.playersHash = playersHash;
                this.targetPairs = targetPairs;
            }
        }
    }
}