using System;
using System.Collections.Generic;
using Core.Sound.presentation;
using Michsky.MUIP;
using ModestTree;
using Shooter.domain;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using Shooter.presentation.UI.Lobby.Store;
using UniRx;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace Shooter.presentation.UI.Lobby.UpcomingRewards
{
    public class UpcomingRewardsModalWindow : MonoBehaviour
    {
        [SerializeField] private StoreScreen store;
        [SerializeField] private UpcomingRewardsItemPool pool;
        
        [Header("Local Components")]
        
        [SerializeField] private ModalWindowManager manager;
        [SerializeField] private GameObject noRewardsPlaceholder;
        [SerializeField] private GameObject morePlaceholder;
        [SerializeField] private ButtonManager moreButton;
        [SerializeField] private Transform root;
        
        [Header("Data")]

        [SerializeField] private LocalizedString titleString;
        [SerializeField] private int maxShownItems = 3;

        [Inject] private WeaponRewardsUseCase weaponRewardsUseCase;
        [Inject] private IWeaponIconRepository weaponIconRepository;
        [Inject] private PlaySoundNavigator playSoundNavigator;

        private readonly List<UpcomingRewardItem> items = new();

        private IDisposable handler;

        private void Start() => moreButton.onClick.AddListener(OpenStore);

        private void OnDisable() => handler.Dispose();

        public void Open()
        {
            handler = weaponRewardsUseCase
                .GetSortedNextRewards()
                .Subscribe(ShowRewards)
                .AddTo(this);
        }

        private void ShowRewards(List<WeaponState> rewards)
        {
            noRewardsPlaceholder.SetActive(rewards.IsEmpty());
            var shownItemsCount = Math.Min(maxShownItems, rewards.Count);
            UpdateItemsCount(shownItemsCount);
            for (var i = 0; i < shownItemsCount; i++)
            {
                var rewardInfo = rewards[i].info;
                var sprite = weaponIconRepository.GetIcon(rewardInfo.id);
                var item = items[i];
                item.SetData(sprite, rewardInfo.availableFromLevel);
                item.SetClickListener(() => OpenReward(rewardInfo.id));
            }

            morePlaceholder.transform.SetAsLastSibling();
            morePlaceholder.SetActive(rewards.Count > maxShownItems);

            manager.titleText = titleString.GetLocalizedString();
            manager.UpdateUI();
            manager.Open();
        }

        private void OpenStore()
        {
            manager.Close();
            store.Open();
        }

        private void OpenReward(long weaponId)
        {
            playSoundNavigator.Play(SoundType.ButtonDefault);
            manager.Close();
            store.Open(weaponId);
        }

        private void UpdateItemsCount(int count)
        {
            while (count > items.Count)
            {
                var newItem = pool.Pop();
                var itemTransform = newItem.transform;
                itemTransform.SetParent(root);
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
    }
}