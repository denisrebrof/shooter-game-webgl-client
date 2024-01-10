using Features.LevelScore.domain;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Features.LevelScore.presentation.ui
{
    public class CurrentLevelScoreReactiveText : MonoBehaviour
    {
        [Inject] private CurrentLevelScoreUseCase currentLevelScoreUseCase;

        [SerializeField] private Text text;
        [SerializeField] private UnityEvent onUpdateText;

        private void Awake()
        {
            if (text == null)
                text = GetComponent<Text>();
        }

        private void Start() => currentLevelScoreUseCase.GetCurrentScoreFlow().Subscribe(UpdateScore).AddTo(this);

        private void UpdateScore(int score)
        {
            text.text = score.ToString();
            onUpdateText?.Invoke();
        }
    }
}