using Features.GlobalScore.domain;
using Features.GlobalScore.domain.model;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Features.GlobalScore.presentation
{
    public class GlobalScoreReactiveText : MonoBehaviour
    {
        [Inject] private IGlobalScoreRepository globalScoreRepository;

        [SerializeField] private Text text;
        [SerializeField] private UnityEvent onUpdateText;

        private void Awake()
        {
            if (text == null)
                text = GetComponent<Text>();
        }

        private void Start() => globalScoreRepository
            .GetScoreFlow(GlobalScoreType.Current)
            .Subscribe(UpdateScore)
            .AddTo(this);

        private void UpdateScore(int score)
        {
            text.text = score.ToString();
            onUpdateText?.Invoke();
        }
    }
}