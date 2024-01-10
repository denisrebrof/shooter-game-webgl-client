using System;
using Core.Analytics.adapter;
using UnityEngine;
using Zenject;

namespace Core.Analytics.screens
{
    public class ScreenStateAnalyticsHandler : MonoBehaviour
    {
        [Inject] private AnalyticsAdapter analytics;
        [SerializeField] private string screenName;

        private void OnEnable() => SendEvent(ScreenAction.Open);

        private void OnDisable() => SendEvent(ScreenAction.Close);

        private void SendEvent(ScreenAction action) => analytics.SendScreenEvent(screenName, action);
    }
}