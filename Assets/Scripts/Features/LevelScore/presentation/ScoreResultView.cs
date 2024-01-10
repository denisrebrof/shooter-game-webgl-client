using Features.LevelScore.domain;
using Features.LevelScore.domain.model;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.LevelScore.presentation
{
    public class ScoreResultView : MonoBehaviour
    {
        [SerializeField] private GameObject bestScoreLabel;
        [SerializeField] private GameObject newBestScoreLabel;
        [SerializeField] private GameObject scoreLabel;

        [SerializeField] private Color defColor;
        [SerializeField] private Color newBestScoreColor;
        [SerializeField] private Text valueText;
        [SerializeField] private Text bestValueText;

        [Inject] private LastLevelScoreUseCase lastLevelScoreUseCase;

        private void Awake()
        {
            if (valueText != null) return;
            valueText = GetComponent<Text>();
        }

        private void OnEnable()
        {
            var lastScore = lastLevelScoreUseCase.GetScore(ScoreType.Last);
            var bestScore = lastLevelScoreUseCase.GetScore(ScoreType.Best);
            var isNewBestScore = bestScore == lastScore;
            bestScoreLabel.SetActive(!isNewBestScore);
            scoreLabel.SetActive(!isNewBestScore);
            newBestScoreLabel.SetActive(isNewBestScore);

            valueText.text = lastScore.ToString();
            valueText.color = isNewBestScore ? newBestScoreColor : defColor;
            bestValueText.text = bestScore.ToString();
            bestValueText.enabled = !isNewBestScore;
        }
    }
}