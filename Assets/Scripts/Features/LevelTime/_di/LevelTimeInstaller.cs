using Features.LevelTime.data;
using Features.LevelTime.domain;
using UnityEngine;
using Zenject;

namespace Features.LevelTime._di
{
    public class LevelTimeInstaller : MonoInstaller
    {
        [SerializeField] private LevelTimerSceneRepository levelTimerSceneRepository;

        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<LevelTimerSceneRepository>()
                .FromInstance(levelTimerSceneRepository)
                .AsSingle();
            Container.BindInterfacesAndSelfTo<CurrentLevelMaxTimeUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelTimeLeftUseCase>().AsSingle();
        }
    }
}