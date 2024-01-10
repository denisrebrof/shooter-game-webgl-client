using System;

namespace Features.Levels.presentation.loader
{
    public class EmptySceneLoadingTransition: LevelSceneLoader.ILevelLoadingTransition
    {
        public void StartAnimation(Action onSceneHidden = null, Action onCompleted = null)
        {
            onSceneHidden?.Invoke();
            onCompleted?.Invoke();
        }
    }
}