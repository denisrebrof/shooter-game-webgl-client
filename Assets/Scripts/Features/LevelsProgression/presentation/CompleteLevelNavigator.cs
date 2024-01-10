using Core.Ads.presentation.InterstitialAdNavigator;
using Core.SDK.GameState;
using Core.Sound.presentation;
using Features.Levels.domain;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Features.LevelsProgression.presentation
{
    public class CompleteLevelNavigator : MonoBehaviour
    {
        [SerializeField] private AudioClip completeLevelSound;
        [SerializeField] private UnityEvent onLevelCompleted;

        [Inject] private CompleteCurrentLevelUseCase completeCurrentLevelUseCase;
        [Inject] private GameStateNavigator gameStateNavigator;
        [Inject] private PlaySoundNavigator playSoundNavigator;

        [Inject(Id = IInterstitialAdNavigator.DefaultInstance)]
        private IInterstitialAdNavigator adNavigator;

        public void CompleteCurrentLevel()
        {
            playSoundNavigator.Play(completeLevelSound);
            gameStateNavigator.SetLevelPlayingState(false);
            completeCurrentLevelUseCase.CompleteCurrentLevel();
            OnLevelCompleted();
        }

        private void OnLevelCompleted() => adNavigator
            .ShowAd()
            .Subscribe(_ => onLevelCompleted?.Invoke())
            .AddTo(this);
    }
}