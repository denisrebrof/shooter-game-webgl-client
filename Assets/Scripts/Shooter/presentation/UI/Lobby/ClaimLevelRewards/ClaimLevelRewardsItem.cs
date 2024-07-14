using System;
using Michsky.MUIP;
using UnityEngine;
using UnityEngine.UI;
using Utils.Pooling;

namespace Shooter.presentation.UI.Lobby.ClaimLevelRewards
{
    public class ClaimLevelRewardsItem : MonoPoolItem
    {
        [SerializeField] private ButtonManager button;
        [SerializeField] private Image icon;

        public void SetData(Sprite sprite)
        {
            icon.enabled = true;
            icon.sprite = sprite;
            icon.preserveAspect = true;
        }

        public void SetClickListener(Action onClick)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onClick.Invoke);
        }
    }
}