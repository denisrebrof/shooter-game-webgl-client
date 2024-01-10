using Features.Levels.presentation;
using Features.Levels.presentation.loader;
using Features.Levels.presentation.ui;
using UnityEngine;
using Zenject;
using static Features.Levels.presentation.loader.LevelSceneLoader;

namespace Features.Levels._di
{
    public class LevelsPresentationInstaller : MonoInstaller
    {
        [SerializeField] private LevelSceneLoader loader;
        [SerializeField] private Component transition;
        [SerializeField] private LevelItem levelItemPrefab;
        [SerializeField] private CurrentLevelLoadingNavigator currentLevelLoadingNavigator;

        public override void InstallBindings()
        {
            //Presentation
            if (transition != null && transition is ILevelLoadingTransition transitionInstance)
                Container.Bind<ILevelLoadingTransition>().FromInstance(transitionInstance).AsSingle();
            else
                Container.Bind<ILevelLoadingTransition>().To<EmptySceneLoadingTransition>().AsSingle();

            Container.Bind<LevelSceneLoader>().FromInstance(loader).AsSingle();
            Container.Bind<LevelLoadingNavigator>().AsSingle();
            Container.Bind<CurrentLevelLoadingNavigator>().FromInstance(currentLevelLoadingNavigator).AsSingle();
            //UI
            Container.Bind<LevelItem.ILevelItemController>().To<DefaultLevelItemController>().AsSingle();
            Container.Decorate<LevelItem.ILevelItemController>().With<LevelItemControllerAnalyticsDecorator>();
            if (levelItemPrefab != null)
            {
                Container.BindFactory<LevelItem, LevelItem.Factory>().FromComponentInNewPrefab(levelItemPrefab);
            }
        }
    }
}