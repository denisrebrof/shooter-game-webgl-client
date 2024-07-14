using Core.Sound.presentation;
using Core.User.domain;
using Michsky.MUIP;
using Shooter.domain;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using Shooter.presentation.UI.Lobby.Store;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Shooter.presentation.UI.Lobby
{
    public class UserInfoScreen : MonoBehaviour
    {
        [SerializeField] private StoreScreen storeScreen;

        [Header("Local Components")]

        [SerializeField] private TMP_Text usernameText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text xpText;
        [SerializeField] private Image progressBar;
        [SerializeField] private Image nextRewardImage;
        [SerializeField] private ButtonManager nextRewardButton;
        [SerializeField] private GameObject nextRewardRoot;

        [Inject] private ICurrentUserNameRepository userNameRepository;
        [Inject] private ILevelProgressionRepository levelProgressionRepository;
        [Inject] private WeaponRewardsUseCase weaponRewardsUseCase;
        [Inject] private IWeaponIconRepository weaponIconRepository;
        [Inject] private PlaySoundNavigator playSoundNavigator;

        private void Start()
        {
            nextRewardRoot.SetActive(false);
            nextRewardImage.enabled = false;
            levelProgressionRepository
                .Get()
                .Subscribe(SetLevelProgression)
                .AddTo(this);
            userNameRepository
                .GetUserNameFlow()
                .Subscribe(userName => usernameText.text = userName)
                .AddTo(this);
            weaponRewardsUseCase
                .GetClosestRewardFlow()
                .Subscribe(SetNextReward)
                .AddTo(this);
        }

        private void SetNextReward(WeaponState reward)
        {
            nextRewardRoot.SetActive(true);
            nextRewardImage.enabled = true;
            nextRewardImage.preserveAspect = true;
            var rewardId = reward.info.id;
            nextRewardImage.sprite = weaponIconRepository.GetIcon(rewardId);
            nextRewardButton.onClick.RemoveAllListeners();
            nextRewardButton.onClick.AddListener(() =>
                {
                    playSoundNavigator.Play(SoundType.ButtonDefault);
                    storeScreen.Open(rewardId);
                }
            );
        }

        private void SetLevelProgression(LevelProgressionData data)
        {
            levelText.text = data.level.ToString();
            var xp = data.xp;
            var nextLevelXp = data.nextLevelXp;
            xpText.text = nextLevelXp > 0 ? $"{xp} / {nextLevelXp} XP" : "";
            var progress = nextLevelXp > 0 ? (float)xp / nextLevelXp : 1f;
            progressBar.fillAmount = progress;
        }
    }
}