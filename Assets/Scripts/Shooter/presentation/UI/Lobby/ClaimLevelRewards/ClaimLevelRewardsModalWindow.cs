using System;
using System.Collections.Generic;
using Core.Sound.presentation;
using Michsky.MUIP;
using Shooter.domain.Repositories;
using Shooter.presentation.UI.Lobby.Store;
using UniRx;
using UnityEngine;
using UnityEngine.Localization;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Shooter.presentation.UI.Lobby.ClaimLevelRewards
{
    public class ClaimLevelRewardsModalWindow : MonoBehaviour
    {
        [SerializeField] private ClaimLevelRewardsItemPool pool;
        [SerializeField] private StoreScreen storeScreen;
        
        [Header("Components Local")]
        
        [SerializeField] private ModalWindowManager manager;
        [SerializeField] private GameObject morePlaceholder;
        [SerializeField] private ButtonManager moreButton;
        [SerializeField] private Transform rewardsRoot;
        
        [Header("Data")]
        
        [SerializeField] private LocalizedString rewardsString;
        [SerializeField] private int maxShownItems = 3;

        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;
        [Inject] private IWeaponIconRepository weaponIconRepository;
        [Inject] private PlaySoundNavigator playSoundNavigator;
        
        private readonly List<ClaimLevelRewardsItem> items = new();

        private void Awake()
        {
            moreButton.onClick.AddListener(OpenStore);
            manager.confirmButton.onClick.AddListener(Claim);
            manager.cancelButton.onClick.AddListener(Claim);
            Debug.Log("Subscribe to level rewards");
            commandsUseCase
                .Request<LevelRewardsResponse>(Commands.UnclaimedLevelRewardsData)
                .Subscribe(ShowLevelRewards)
                .AddTo(this);
        }

        private void ShowLevelRewards(LevelRewardsResponse data)
        {
            var rewardIds = data.weaponRewards;
            var shownItemsCount = Math.Min(maxShownItems, rewardIds.Count);
            UpdateItemsCount(shownItemsCount);
            for (var i = 0; i < shownItemsCount; i++)
            {
                var weaponId = rewardIds[i];
                var sprite = weaponIconRepository.GetIcon(weaponId);
                var item = items[i];
                item.SetData(sprite);
                item.SetClickListener(() => OpenReward(weaponId));
            }

            morePlaceholder.transform.SetAsLastSibling();
            morePlaceholder.SetActive(rewardIds.Count > maxShownItems);

            var levelsText = data.currentLevel > data.lastLevel + 1
                ? $"{data.lastLevel} - {data.currentLevel}"
                : data.currentLevel.ToString();
            var title = rewardsString
                .GetLocalizedString()
                .Replace("$", levelsText);
            manager.titleText = title;
            
            manager.UpdateUI();
            manager.Open();
        }
        
        private void OpenStore()
        {
            playSoundNavigator.Play(SoundType.ButtonDefault);
            manager.Close();
            storeScreen.Open();
            Claim();
        }
        
        private void OpenReward(long weaponId)
        {
            playSoundNavigator.Play(SoundType.ButtonDefault);
            manager.Close();
            storeScreen.Open(weaponId);
            Claim();
        }

        private void Claim()
        {
            // manager.Close();
            commandsUseCase
                .Request<int>(Commands.ClaimLevelRewards)
                .Subscribe()
                .AddTo(this);
        }
        
        private void UpdateItemsCount(int count)
        {
            while (count > items.Count)
            {
                var newItem = pool.Pop();
                var itemTransform = newItem.transform;
                itemTransform.SetParent(rewardsRoot);
                itemTransform.localScale = Vector3.one;
                items.Add(newItem);
            }

            while (count < items.Count)
            {
                var extraItem = items[0];
                pool.Return(extraItem);
                items.RemoveAt(0);
            }
        }

        [Serializable]
        private struct LevelRewardsResponse
        {
            public int lastLevel;
            public int currentLevel;
            public List<long> weaponRewards;
        }
    }
}