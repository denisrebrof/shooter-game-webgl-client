using Features.LevelScore.data;
using Features.LevelScore.domain;
using UnityEngine;
using Zenject;

namespace Features.LevelScore._di
{
    [CreateAssetMenu(menuName = "Installers/LevelScoreInstaller")]
    public class LevelScoreInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LevelScoreLocalDataSource>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelScoreRepository>().AsSingle();

            Container.BindInterfacesAndSelfTo<LastLevelScoreUseCase>().AsSingle();
            Container.Bind<LevelLeaderboardUseCase>().ToSelf().AsSingle();
            Container.Bind<LastLevelLeaderBoardUseCase>().ToSelf().AsSingle();
            Container.Bind<CurrentLevelScoreUseCase>().ToSelf().AsSingle();
            Container.Bind<UpdateCurrentLevelScoreUseCase>().ToSelf().AsSingle();
        }
    }
}