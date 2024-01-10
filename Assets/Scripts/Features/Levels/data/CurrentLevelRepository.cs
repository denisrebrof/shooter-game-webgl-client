using System;
using Features.Levels.domain.model;
using Features.Levels.domain.repositories;
using UniRx;
using Zenject;

namespace Features.Levels.data
{
    internal class CurrentLevelRepository : ICurrentLevelRepository
    {
        private ILevelsRepository levelsRepository;
        private ICurrentLevelIdDataSource currentLevelIdDataSource;
        private IDefaultLevelIdDao defaultLevelIdDao;

        private BehaviorSubject<int> currentLevelIdSubject;

        private int CurrentLevelId => currentLevelIdDataSource.HasCurrentLevelId()
            ? currentLevelIdDataSource.GetCurrentLevelId()
            : defaultLevelIdDao.GetDefaultLevelId();
        
        private int PrevLevelId => currentLevelIdDataSource.HasCurrentLevelId()
            ? currentLevelIdDataSource.GetPrevLevelId()
            : defaultLevelIdDao.GetDefaultLevelId();

        [Inject]
        public CurrentLevelRepository(
            ILevelsRepository levelsRepository,
            ICurrentLevelIdDataSource currentLevelIdDataSource,
            IDefaultLevelIdDao defaultLevelIdDao)
        {
            this.levelsRepository = levelsRepository;
            this.currentLevelIdDataSource = currentLevelIdDataSource;
            this.defaultLevelIdDao = defaultLevelIdDao;
            currentLevelIdSubject = new BehaviorSubject<int>(CurrentLevelId);
        }

        public void SetCurrentLevel(int levelId)
        {
            currentLevelIdDataSource.SetCurrentLevelId(levelId);
            currentLevelIdSubject.OnNext(levelId);
        }

        public Level GetCurrentLevel() => levelsRepository.GetLevel(CurrentLevelId);
        public Level GetPrevLevel() => levelsRepository.GetLevel(PrevLevelId);

        public IObservable<Level> GetCurrentLevelFlow() => currentLevelIdSubject.Select(levelsRepository.GetLevel);

        public interface ICurrentLevelIdDataSource
        {
            public bool HasCurrentLevelId();
            public int GetCurrentLevelId();
            public int GetPrevLevelId();
            public void SetCurrentLevelId(int id);
        }

        public interface IDefaultLevelIdDao
        {
            public int GetDefaultLevelId();
        }
    }
}