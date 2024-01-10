using System;
using Features.GlobalScore.domain.model;
using Plugins.FileIO;
using UniRx;
using Zenject;

namespace Features.GlobalScore.data
{
    public class GlobalScoreLocalDataSource
    {
        private const string Key = "GlobalScore";
        private const string PrevKey = "PrevGlobalScore";

        private readonly ReactiveProperty<int> scoreFlow;
        private readonly ReactiveProperty<int> prevScoreFlow;

        [Inject]
        public GlobalScoreLocalDataSource()
        {
            scoreFlow = new ReactiveProperty<int>(Score);
            prevScoreFlow = new ReactiveProperty<int>(Score);
        }
        
        private int PrevScore
        {
            get => GetStoredScore(PrevKey);
            set => UpdateScore(PrevKey, prevScoreFlow, value);
        }

        private int Score
        {
            get => GetStoredScore(Key);
            set => UpdateScore(Key, scoreFlow, value);
        }

        public int GetScore(GlobalScoreType type) => type switch
        {
            GlobalScoreType.Previous => PrevScore,
            GlobalScoreType.Current => Score,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };


        public IObservable<int> GetScoreFlow(GlobalScoreType type) => type switch
        {
            GlobalScoreType.Previous => scoreFlow.Scan((prev, curr) => prev),
            GlobalScoreType.Current => scoreFlow,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        public void SendScore(int score)
        {
            PrevScore = Score;
            Score = score;
        }
        
        private int GetStoredScore(string key) => LocalStorageIO.HasKey(key) ? LocalStorageIO.GetInt(key) : 0;

        private void UpdateScore(string key, IReactiveProperty<int> handler, int newValue)
        {
            LocalStorageIO.SetInt(key, newValue);
            handler.Value = newValue;
            LocalStorageIO.Save();
        }
    }
}