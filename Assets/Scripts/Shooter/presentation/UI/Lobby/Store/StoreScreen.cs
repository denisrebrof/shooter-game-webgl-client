using System.Collections;
using System.Collections.Generic;
using Core.Sound.presentation;
using Michsky.MUIP;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Zenject;
using static Shooter.presentation.UI.Lobby.Store.StoreItemView;

namespace Shooter.presentation.UI.Lobby.Store
{
    public class StoreScreen : MonoBehaviour
    {
        [Inject] private IWeaponStoreRepository weaponStoreRepository;
        [Inject] private ILevelProgressionRepository levelProgressionRepository;
        [Inject] private PlaySoundNavigator playSoundNavigator;

        [SerializeField] private WeaponInfoModalWindow weaponInfoModalWindow;
        [SerializeField] private StoreItemPool itemPool;

        [Header("Local Components")]

        [SerializeField] private ModalWindowManager storeManager;
        [SerializeField] private RectTransform storeRoot;

        [Header("Settings")]

        [SerializeField] private float openWeaponDelay = 0.6f;

        private readonly Dictionary<long, StoreItemViewData> viewData = new();
        private readonly Dictionary<long, StoreItemView> views = new();

        private int playerLevel;

        private void Start() =>
            levelProgressionRepository
                .Get()
                .Select(data => data.level)
                .Subscribe(SubscribeToWeaponsData)
                .AddTo(this);

        private void OnDisable() => StopAllCoroutines();

        public void Open() => storeManager.Open();

        public void Open(long weaponId)
        {
            storeManager.Open();
            StartCoroutine(OpenWeaponWihDelayCoroutine(weaponId));
        }

        private IEnumerator OpenWeaponWihDelayCoroutine(long weaponId)
        {
            yield return new WaitForSeconds(openWeaponDelay);
            weaponInfoModalWindow.Open(weaponId);
        }

        private void SubscribeToWeaponsData(int level)
        {
            playerLevel = level;
            weaponStoreRepository
                .GetWeaponsData()
                .Subscribe(UpdateData)
                .AddTo(this);
        }

        private void UpdateData(WeaponsData data)
        {
            var primaryId = data.primaryId;
            var secondaryId = data.secondaryId;
            foreach (var weaponState in data.weapons)
            {
                var weaponId = weaponState.info.id;
                var itemViewData = new StoreItemViewData
                {
                    State = weaponState,
                    PlayerLevel = playerLevel,
                    IsPrimary = primaryId == weaponId,
                    IsSecondary = secondaryId == weaponId,
                };
                if (viewData.TryGetValue(weaponId, out var existingData))
                {
                    if (existingData != itemViewData)
                    {
                        var existingView = views[weaponId];
                        existingView.Setup(itemViewData);
                        existingView.SetClickListener(() =>
                        {
                            playSoundNavigator.Play(SoundType.ButtonDefault);
                            weaponInfoModalWindow.Open(weaponId);
                        });
                    }

                    continue;
                }

                var itemView = GetNewItemView();
                viewData[weaponId] = itemViewData;
                views[weaponId] = itemView;
                itemView.Setup(itemViewData);
                itemView.SetClickListener(() =>
                {
                    playSoundNavigator.Play(SoundType.ButtonDefault);
                    weaponInfoModalWindow.Open(weaponId);
                });
            }
        }

        private StoreItemView GetNewItemView()
        {
            var view = itemPool.Pop();
            var viewTransform = view.transform;
            viewTransform.SetParent(storeRoot);
            viewTransform.localScale = Vector3.one;
            return view;
        }
    }
}