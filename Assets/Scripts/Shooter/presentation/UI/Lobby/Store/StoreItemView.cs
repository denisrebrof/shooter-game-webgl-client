using System;
using Michsky.MUIP;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Utils.Pooling;
using Zenject;

namespace Shooter.presentation.UI.Lobby.Store
{
    public class StoreItemView : MonoPoolItem
    {
        [Inject] private IWeaponIconRepository iconRepository;

        [Header("Common")]

        [SerializeField] private Image graphics;
        [SerializeField] private ButtonManager button;
        [SerializeField] private LocalizeStringEvent weaponNameEvent;
        [SerializeField] private string weaponNameTable;

        [Header("Slot")]

        [SerializeField] private GameObject slotRoot;
        [SerializeField] private LocalizeStringEvent slotEvent;
        [SerializeField] private string slotTable;
        [SerializeField] private string primarySlotKey;
        [SerializeField] private string secondarySlotKey;

        [Header("Cost")]

        [SerializeField] private Sprite purchaseIcon;
        [SerializeField] private Sprite upgradeIcon;
        [SerializeField] private TMP_Text costText;
        [SerializeField] private Image costIcon;
        [SerializeField] private GameObject costRoot;

        [Header("Lock")]

        [SerializeField] private TMP_Text requiredLevelText;
        [SerializeField] private GameObject lockedMarkObject;

        [Header("Upgrades")]

        [SerializeField] private RectTransform upgradeProgress;
        [SerializeField] private RectTransform upgradeProgressBackground;
        [SerializeField] private int progressImageLevelsCount = 5;
        private float progressImageWidth;
        private float progressImageLevelLength;

        private Action actions;

        private void Awake()
        {
            progressImageWidth = upgradeProgressBackground.rect.width;
            progressImageLevelLength = 1f / progressImageLevelsCount;
            button.onClick.AddListener(() =>
            {
                Debug.Log($"Click actions: {actions!=null}");
                actions?.Invoke();
            });
        }

        public void SetClickListener(Action onClick)
        {
            actions = null;
            actions = onClick;
        }

        public void Setup(StoreItemViewData data)
        {
            var state = data.State;
            var info = state.info;
            SetIcon(info.id);
            SetSlot(data.IsPrimary, data.IsSecondary);
            SetProgressRectLevel(upgradeProgress, state.currentLevel);
            SetProgressRectLevel(upgradeProgressBackground, info.settingsLevels.Length);
            SetName(info.nameLocalizationKey);

            var locked = !state.Purchased && info.availableFromLevel > data.PlayerLevel;
            lockedMarkObject.SetActive(locked);
            requiredLevelText.text = info.availableFromLevel.ToString();

            costRoot.SetActive(!state.Purchased || state.Upgradable);
            costIcon.sprite = state.Purchased ? upgradeIcon : purchaseIcon;
            costText.text = state.Cost.ToString();
        }

        private void SetSlot(bool isPrimary, bool isSecondary)
        {
            var slotVisible = isPrimary || isSecondary;
            slotRoot.SetActive(slotVisible);
            if (!slotVisible)
                return;

            var slotKey = isPrimary ? primarySlotKey : secondarySlotKey;
            slotEvent.StringReference.SetReference(slotTable, slotKey);
        }

        private void SetIcon(long weaponId)
        {
            graphics.enabled = true;
            graphics.sprite = iconRepository.GetIcon(weaponId);
            graphics.preserveAspect = true;
        }

        private void SetProgressRectLevel(RectTransform target, int level)
        {
            var progressWidth = Mathf.Clamp01(progressImageLevelLength * level) * progressImageWidth;
            target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, progressWidth);
        }

        private void SetName(string localizationKey) =>
            weaponNameEvent
                .StringReference
                .SetReference(weaponNameTable, localizationKey);

        public struct StoreItemViewData
        {
            public WeaponState State;
            public int PlayerLevel;
            public bool IsPrimary;
            public bool IsSecondary;

            public bool Equals(StoreItemViewData other)
            {
                return State.Equals(other.State) && PlayerLevel == other.PlayerLevel && IsPrimary == other.IsPrimary && IsSecondary == other.IsSecondary;
            }

            public override bool Equals(object obj)
            {
                return obj is StoreItemViewData other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(State, PlayerLevel, IsPrimary, IsSecondary);
            }

            public static bool operator ==(StoreItemViewData left, StoreItemViewData right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(StoreItemViewData left, StoreItemViewData right)
            {
                return !left.Equals(right);
            }
        }
    }
}