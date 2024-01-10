using Core.SDK.HappyTime;
using UnityEngine;
using Utils.PlayerTrigger;
using Zenject;

namespace Features.LevelsProgression.presentation
{
    public class CompleteLevelTrigger : PlayerTriggerBase
    {
        [SerializeField, Range(0f, 1f)] private float happyTimeIntensity = 0.5f;
        
        [Inject] private CompleteLevelNavigator navigator;
        [Inject] private IHappyTimeController controller;

        protected override void OnPlayerEntersTrigger()
        {
            controller.SetHappyTime(happyTimeIntensity);
            navigator.CompleteCurrentLevel();
        }

        protected override void OnPlayerExitTrigger()
        {
            //do nothing
        }
    }
}