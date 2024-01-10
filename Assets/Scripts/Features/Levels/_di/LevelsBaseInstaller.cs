using Features.Levels.data;
using Features.Levels.data.dao;
using Features.Levels.domain;
using Features.Levels.domain.repositories;
using UnityEngine;
using Zenject;

namespace Features.Levels._di
{
    [CreateAssetMenu(menuName = "Installers/LevelsBaseInstaller")]
    public class LevelsBaseInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private PrefabLevelsSORepository levelsRepository;
        public override void InstallBindings()
        {
            //Daos
            Container.Bind<ILevelCompletedStateDao>()
#if PLAYER_PREFS_STORAGE
                .To<PlayerPrefsLevelCompletedStateDao>()
#else
                .To<LocalStorageLevelCompletedStateDao>()
#endif
                .AsSingle();

            Container.Bind<CurrentLevelRepository.ICurrentLevelIdDataSource>()
#if PLAYER_PREFS_STORAGE
                .To<PlayerPrefsCurrentLevelIdDao>()
#else
                .To<LocalStorageCurrentLevelIdDataSource>()
#endif
                .AsSingle();


            Container.Bind<CurrentLevelRepository.IDefaultLevelIdDao>().To<HardcodedDefaultLevelIdDao>().AsSingle();
            //Repositories
            Container.BindInterfacesAndSelfTo<PrefabLevelsSORepository>().FromInstance(levelsRepository).AsCached();
            Container.Bind<ICurrentLevelRepository>().To<CurrentLevelRepository>().AsSingle();
            Container.Bind<ILevelCompletedStateRepository>().To<LevelCompletedStateRepository>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<LevelAnalyticsRepository>().AsSingle();
            
            //UseCases
            Container.Bind<SetNextCurrentLevelUseCase>().ToSelf().AsSingle();
            Container.Bind<GetCompletedLevelsUseCase>().ToSelf().AsSingle();
        }
    }
}