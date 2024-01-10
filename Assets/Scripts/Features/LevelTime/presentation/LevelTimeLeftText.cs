using System;
using Features.LevelTime.domain;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.LevelTime.presentation
{
    public class LevelTimeLeftText: MonoBehaviour
    {
        [SerializeField] private Text text;
        [Inject] private LevelTimeLeftUseCase levelTimeLeftUseCase;

        private void Awake()
        {
            if (text == null)
                text = GetComponent<Text>();
        }

        private void Start() => levelTimeLeftUseCase.GetTimeLeftFlow().Subscribe(UpdateTimer).AddTo(this);

        private void UpdateTimer(long timer)
        {
            text.text = TimeSpan.FromMilliseconds(timer).ToString(@"mm\:ss");
        }
    }
}