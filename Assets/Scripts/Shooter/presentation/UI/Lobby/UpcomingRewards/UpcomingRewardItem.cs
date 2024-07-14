using System;
using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Utils.Pooling;

namespace Shooter.presentation.UI.Lobby.UpcomingRewards
{
    public class UpcomingRewardItem : MonoPoolItem
    {
        [SerializeField] private ButtonManager button;
        [SerializeField] private Image icon;
        [SerializeField] private LocalizedString levelString;
        [SerializeField] private TMP_Text levelText;

        public void SetData(Sprite sprite, int level)
        {
            icon.enabled = true;
            icon.sprite = sprite;
            icon.preserveAspect = true;
            levelText.text = levelString.GetLocalizedString().Replace("$", level.ToString());
        }

        public void SetClickListener(Action onClick)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onClick.Invoke);
        }
    }
}