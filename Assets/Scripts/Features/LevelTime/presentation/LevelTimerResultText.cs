using System;
using Features.LevelTime.domain;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.LevelTime.presentation
{
    public class LevelTimerResultText: MonoBehaviour
    {
        [SerializeField] private Text text;
        [Inject] private ILevelTimerRepository levelTimerRepository;

        private void Awake()
        {
            if (text == null)
                text = GetComponent<Text>();
        }

        private void OnEnable()
        {
            var timer = levelTimerRepository.GetTimerResult();
            text.text = TimeSpan.FromMilliseconds(timer).ToString(@"mm\:ss");
        }
    }
}