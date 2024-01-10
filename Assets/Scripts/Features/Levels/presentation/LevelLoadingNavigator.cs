using System;
using Features.Levels.domain.repositories;
using Features.Levels.presentation.loader;
using UnityEngine;
using Zenject;

namespace Features.Levels.presentation
{
    public class LevelLoadingNavigator
    {
        [Inject] private ILevelSceneObjectRepository repository;
        [Inject] private LevelSceneLoader loader;

        public Action<int> LevelLoaded;

        public void LoadLevel(int levelId)
        {
            Debug.Log("Load level " + levelId);
            var scene = repository.GetLevelScene(levelId);
            loader.LoadLevel(scene);
            LevelLoaded?.Invoke(levelId);
        }
    }
}