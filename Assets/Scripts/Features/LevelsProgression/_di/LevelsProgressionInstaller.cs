using Features.Levels.domain;
using Features.LevelsProgression.presentation;
using UnityEngine;
using Zenject;

namespace Features.LevelsProgression._di
{
    public class LevelsProgressionInstaller : MonoInstaller
    {
        [SerializeField] private CompleteLevelNavigator completeLevelNavigator;

        public override void InstallBindings()
        {
            Container.Bind<CompleteCurrentLevelUseCase>().ToSelf().AsSingle();
            Container.BindInstance(completeLevelNavigator).AsSingle();
        }
    }
}