using System;
using UniRx;
using UnityEngine;

namespace Core.SDK.GameState
{
    public class GameStateNavigator
    {
        private ReactiveProperty<bool> menuShownState = new(false);
        private ReactiveProperty<bool> levelPlayingState = new(true);

        public void SetMenuShownState(bool enabled)
        {
            // Debug.Log("GameStateNavigator SetMenuShownState " + enabled);
            menuShownState.Value = enabled;
        }

        public void SetLevelPlayingState(bool enabled)
        {
            // Debug.Log("GameStateNavigator SetLevelPlayingState " + enabled);
            levelPlayingState.Value = enabled;
        }

        public IObservable<GameState> GetGameState() => Observable.CombineLatest(
            menuShownState,
            levelPlayingState,
            (menuShown, levelPlaying) => !menuShown && levelPlaying
        ).Select(playing => playing ? GameState.Active : GameState.Disabled).DistinctUntilChanged();
    }
}