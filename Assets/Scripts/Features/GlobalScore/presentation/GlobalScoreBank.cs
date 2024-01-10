using Features.GlobalScore.domain;
using Features.GlobalScore.domain.model;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.GlobalScore.presentation
{
    public class GlobalScoreBank : MonoBehaviour
    {
        [SerializeField] private Transform fluid;
        [SerializeField] private int maxScore;
        [Inject] private IGlobalScoreRepository globalScoreRepository;

        private void Start() => globalScoreRepository
            .GetScoreFlow(GlobalScoreType.Current)
            .Subscribe(DisplayScore)
            .AddTo(this);

        private void DisplayScore(int score)
        {
            if (maxScore == 0) return;
            var height = Mathf.Clamp((float)score / maxScore, 0f, 1f);
            fluid.localScale = new Vector3(1f, height, 1f);
        }
    }
}