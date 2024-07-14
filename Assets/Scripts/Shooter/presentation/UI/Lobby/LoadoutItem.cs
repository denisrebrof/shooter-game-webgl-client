using System;
using Michsky.MUIP;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Zenject;
using static UnityEngine.RectTransform;

namespace Shooter.presentation.UI.Lobby
{
    public class LoadoutItem : MonoBehaviour
    {
        [Inject] private IWeaponIconRepository iconRepository;

        [SerializeField] private ButtonManager button;
        [SerializeField] private GameObject contentRoot;
        [SerializeField] private Image graphics;
        [SerializeField] private RectTransform upgradeProgress;
        [SerializeField] private RectTransform upgradeProgressBackground;
        [SerializeField] private int progressImageLevelsCount = 5;
        [SerializeField] private LocalizeStringEvent weaponNameEvent;
        [SerializeField] private string weaponNameTable;

        private float progressImageWidth;
        private float progressImageLevelLength;

        private void Awake()
        {
            contentRoot.SetActive(false);
            progressImageWidth = upgradeProgressBackground.rect.width;
            progressImageLevelLength = 1f / progressImageLevelsCount;
        }

        public void SetClickAction(Action onClick) =>
            button
                .onClick
                .AddListener(onClick.Invoke);

        public void Setup(WeaponState state)
        {
            var info = state.info;
            SetIcon(info.id);
            SetProgressRectLevel(upgradeProgress, state.currentLevel);
            SetProgressRectLevel(upgradeProgressBackground, info.settingsLevels.Length);
            SetName(info.nameLocalizationKey);
            contentRoot.SetActive(true);
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
            target.SetSizeWithCurrentAnchors(Axis.Horizontal, progressWidth);
        }

        private void SetName(string localizationKey) =>
            weaponNameEvent
                .StringReference
                .SetReference(weaponNameTable, localizationKey);
    }
}