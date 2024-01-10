using System;
using Features.LevelTime.domain;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace Features.LevelTime.data
{
    public class LevelTimerSceneRepository : MonoBehaviour, ILevelTimerRepository
    {
        private readonly ReactiveProperty<long> timerFlow = new(0);
        private long startTime;
        private long timerResult;
        private bool paused;

        [CanBeNull] private IDisposable timerDisposable;

        public IObservable<long> GetTimerFlow() => timerFlow;

        public long GetTimerResult() => timerResult;

        public void StartTimer()
        {
            startTime = CurrentTime;
            timerFlow.Value = 0;
            timerDisposable?.Dispose();
            SetPaused(false);
            timerDisposable = Observable
                .Timer(TimeSpan.FromSeconds(1))
                .Repeat()
                .Where(_ => !paused)
                .Subscribe(_ => timerFlow.Value = CurrentTime - startTime);
        }

        public void SetPaused(bool pausedState) => paused = pausedState;

        public void StopTimer()
        {
            timerDisposable?.Dispose();
            timerResult = timerFlow.Value;
        }

        private static long CurrentTime => DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }
}