using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Plugins.Platforms.YSDK
{
    public class YandexSDK : MonoBehaviour
    {
        public static YandexSDK instance;

        [DllImport("__Internal")]
        private static extern void GetUserData();

        [DllImport("__Internal")]
        private static extern void RequestPlayerId();
        
        [DllImport("__Internal")]
        private static extern void RequestPurchases();

        [DllImport("__Internal")]
        private static extern string GetLang();

        [DllImport("__Internal")]
        private static extern void ShowFullscreenAd();
        
        [DllImport("__Internal")]
        private static extern void InvokeGRA();

        /// <summary>
        /// Returns an int value which is sent to index.html
        /// </summary>
        /// <param name="placement"></param>
        /// <returns></returns>
        [DllImport("__Internal")]
        private static extern int ShowRewardedAd(string placement);

        // [DllImport("__Internal")]
        // private static extern void GerReward();

        [DllImport("__Internal")]
        private static extern void AuthenticateUser();

        [DllImport("__Internal")]
        private static extern void Purchase(string id);

        [DllImport("__Internal")]
        private static extern void Hit(string id);

        [DllImport("__Internal")]
        private static extern string GetDeviceType();

        public UserData user;
        public event Action onUserDataReceived;

        public event Action<string> onPlayerIdReceived;
        public event Action onInterstitialShown;
        public event Action<string> onInterstitialFailed;
        
        public event Action<string> onPurchaseReceived;
        public event Action<string> onPurchased;
        public event Action<string> onPurchaseFailed;

        /// <summary>
        /// Пользователь открыл рекламу
        /// </summary>
        public event Action<int> onRewardedAdOpened;

        /// <summary>
        /// Пользователь должен получить награду за просмотр рекламы
        /// </summary>
        public event Action<string> onRewardedAdReward;

        /// <summary>
        /// Пользователь закрыл рекламу
        /// </summary>
        public event Action<int> onRewardedAdClosed;

        /// <summary>
        /// Вызов/просмотр рекламы повлёк за собой ошибку
        /// </summary>
        public event Action<string> onRewardedAdError;

        /// <summary>
        public Queue<int> rewardedAdPlacementsAsInt = new();

        public Queue<string> rewardedAdsPlacements = new();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ShowInterstitial()
        {
            ShowFullscreenAd();
        }

        public void InvokeGameReadyApi()
        {
            InvokeGRA();
        }

        public string GetLanguage()
        {
            return GetLang();
        }

        public void RequestPlayerIndentifier()
        {
            RequestPlayerId();
        }

        public bool GetIsOnDesktop() => !GetDeviceType().ToLower().Contains("mobile");

        public void ShowRewarded(string placement)
        {
            rewardedAdPlacementsAsInt.Enqueue(ShowRewardedAd(placement));
            rewardedAdsPlacements.Enqueue(placement);
        }

        public void AddHit(string eventName)
        {
            Hit(eventName);
        }
        
        
        public void OnInterstitialShown()
        {
            if (onInterstitialShown != null) onInterstitialShown.Invoke();
        }

        public void OnInterstitialFailed(string error)
        {
            if (onInterstitialFailed != null) onInterstitialFailed(error);
        }

        public void OnRewardedOpen(int placement)
        {
            onRewardedAdOpened(placement);
        }

        public void OnRewarded(int placement)
        {
            if (placement == rewardedAdPlacementsAsInt.Dequeue())
            {
                onRewardedAdReward.Invoke(rewardedAdsPlacements.Dequeue());
            }
        }

        public void OnRewardedClose(int placement)
        {
            onRewardedAdClosed(placement);
        }

        public void OnRewardedError(string placement)
        {
            onRewardedAdError(placement);
            rewardedAdsPlacements.Clear();
            rewardedAdPlacementsAsInt.Clear();
        }

        public void OnPurchaseFailed(string pid)
        {
            if (onPurchaseFailed != null) onPurchaseFailed.Invoke(pid);
        }
        
        public void OnPurchaseSuccess(string pid)
        {
            if (onPurchased != null) onPurchased.Invoke(pid);
        }
        
        public void OnPurchaseReceived(string pid)
        {
            if (onPurchaseReceived != null) onPurchaseReceived.Invoke(pid);
        }

        public void OnHandlePlayerId(string playerId)
        {
            Debug.Log("Handle PId in YandexSDK");
            onPlayerIdReceived?.Invoke(playerId);
        }
    }

    public struct UserData
    {
        public string id;
        public string name;
        public string avatarUrlSmall;
        public string avatarUrlMedium;
        public string avatarUrlLarge;
    }
}