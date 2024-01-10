using System;
using System.Collections.Generic;
using System.Linq;
using Features.Levels.domain.model;
using Features.Levels.domain.repositories;
using UniRx;
using Zenject;

namespace Features.Levels.domain
{
    public class GetCompletedLevelsUseCase
    {
        [Inject] private ILevelCompletedStateRepository completedStateRepository;
        [Inject] private ILevelsRepository levelsRepository;

        public IEnumerable<Level> GetCompletedLevels() => levelsRepository
            .GetLevels()
            .Where(level => completedStateRepository.GetLevelCompletedState(level.ID))
            .ToList();

        public IObservable<List<Level>> GetCompletedLevelsFlow() => levelsRepository
            .GetLevels()
            .Select(GetLevelCompletedState)
            .CombineLatest()
            .Select(states =>
                states
                    .Where(state => state.Completed)
                    .Select(state => state.Level)
                    .ToList()
            );

        private IObservable<LevelCompletedState> GetLevelCompletedState(Level level) => completedStateRepository
            .GetLevelCompletedStateFlow(level.ID)
            .Select(completed =>
                new LevelCompletedState
                {
                    Level = level,
                    Completed = completed
                }
            );

        private struct LevelCompletedState
        {
            public Level Level;
            public bool Completed;
        }
    }
}