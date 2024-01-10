using Core.Analytics.adapter;
using Core.SDK.SDKType;
using UnityEngine;
using Zenject;

namespace Core.Analytics
{
    public class AnalyticsPlatformSetup : MonoBehaviour
    {
        [Inject] private AnalyticsAdapter analyticsAdapter;

        private void Awake() => analyticsAdapter.SetPlatform(SDKProvider.GetSDK());
    }
}