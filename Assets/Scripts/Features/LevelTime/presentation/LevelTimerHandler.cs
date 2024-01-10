using Core.SDK.GameState;
using Features.Levels.presentation;
using Features.LevelTime.domain;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Features.LevelTime.presentation
{
    public class LevelTimerHandler : MonoBehaviour
    {
        [Inject] private GameStateNavigator gameStateNavigator;
        [Inject] private LevelLoadingNavigator levelLoadingNavigator;
        [Inject] private ILevelTimerRepository levelTimerRepository;
        [Inject] private ILevelTimeRepository levelTimeRepository;
        [Inject] private LevelTimeLeftUseCase levelTimeLeftUseCase;

        [SerializeField] private UnityEvent onExpire;

        private void Start()
        {
            levelTimerRepository.StartTimer();

            gameStateNavigator
                .GetGameState()
                .Select(state => state != GameState.Active)
                .Subscribe(levelTimerRepository.SetPaused)
                .AddTo(this);

            levelLoadingNavigator.LevelLoaded += levelId =>
            {
                var maxTime = levelTimeRepository.GetMaxTime(levelId);
                if (maxTime <= 0) return;
                levelTimerRepository.StartTimer();
            };

            levelTimeLeftUseCase
                .GetTimeLeftFlow()
                .Select(timeLeft => timeLeft <= 0)
                .DistinctUntilChanged()
                .Where(timerExpired => timerExpired)
                .Subscribe(_ => onExpire?.Invoke())
                .AddTo(this);
        }
    }
}