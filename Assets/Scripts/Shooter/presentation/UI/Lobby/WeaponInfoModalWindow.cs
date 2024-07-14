using System;
using Core.Sound.presentation;
using InfimaGames.LowPolyShooterPack;
using Michsky.MUIP;
using Shooter.domain;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
using Utils.Reactive;
using Zenject;
using Disposable = UniRx.Disposable;

namespace Shooter.presentation.UI.Lobby
{
    public class WeaponInfoModalWindow : MonoBehaviour
    {
        [Inject] private IWeaponIconRepository iconRepository;
        [Inject] private WeaponFullDataUseCase weaponFullDataUseCase;
        [Inject] private WeaponStoreOperationsUseCase operationsUseCase;
        [Inject] private SetWeaponSlotUseCase setWeaponSlotUseCase;
        [Inject] private PlaySoundNavigator playSoundNavigator;

        [Header("Common")]

        [SerializeField] private NotificationManager actionResultsManager;
        [SerializeField] private ModalWindowManager manager;
        [SerializeField] private Image graphics;
        [SerializeField] private TableReference weaponNameTable;
        [SerializeField] private TMP_Text description;

        [Header("Weapon Stats")]

        [SerializeField] private TMP_Text ammoAmountText;
        [SerializeField] private TMP_Text rpmAmountText;
        [SerializeField] private TMP_Text damageAmountText;

        [Header("Purchase Button")]

        [SerializeField] private LocalizedString purchaseAvailableText;
        [SerializeField] private LocalizedString purchaseLockedText;
        [SerializeField] private Sprite lockedIcon;
        [SerializeField] private Sprite avaliableForPurchaseIcon;
        [SerializeField] private GameObject purchaseButtonRoot;
        [SerializeField] private ButtonManager purchaseButton;

        [Header("Upgrade Button")]

        [SerializeField] private LocalizedString upgradeText;
        [SerializeField] private GameObject upgradeButtonRoot;
        [SerializeField] private ButtonManager upgradeButton;

        [Header("Notifications")]

        [SerializeField] private Sprite actionSuccessIcon;
        [SerializeField] private Sprite actionFailureIcon;
        [SerializeField] private LocalizedString purchasedNotificationTitleText;
        [SerializeField] private LocalizedString purchasedNotificationDescText;
        [SerializeField] private LocalizedString purchaseFailedTitleText;
        [SerializeField] private LocalizedString purchaseFailedDescText;
        [SerializeField] private LocalizedString upgradedNotificationTitleText;
        [SerializeField] private LocalizedString upgradedNotificationDescText;
        [SerializeField] private LocalizedString upgradeFailedTitleText;
        [SerializeField] private LocalizedString upgradeFailedDescText;

        [Header("Slots")]

        [SerializeField] private LocalizedString setSlotText;
        [SerializeField] private LocalizedString primarySlotText;
        [SerializeField] private LocalizedString secondarySlotText;
        [SerializeField] private GameObject setSlotButtonRoot;
        [SerializeField] private ButtonManager setSlotButton;

        [Header("Select slot window")]

        [SerializeField] private ModalWindowManager slotSelectionManager;
        [SerializeField] private ButtonManager setPrimarySlotButton;
        [SerializeField] private ButtonManager setSecondarySlotButton;
        [SerializeField] private LocalizedString slotSelectionModalWindowTitleText;
        [SerializeField] private LocalizedString slotSelectedNotificationTitleText;
        [SerializeField] private LocalizedString slotSelectedNotificationDescText;
        [SerializeField] private LocalizedString slotSelectionFailedTitleText;
        [SerializeField] private LocalizedString slotSelectionFailedDescText;

        [Header("Upgrades")]

        [SerializeField] private RectTransform upgradeProgress;
        [SerializeField] private RectTransform upgradeProgressBackground;
        [SerializeField] private int progressImageLevelsCount = 5;

        private float progressImageWidth;
        private float progressImageLevelLength;

        private IDisposable dataLoader = Disposable.Empty;
        private CompositeDisposable weaponContext = new();
        private long weaponId;

        public void Open(long weaponId)
        {
            if (progressImageWidth == 0) progressImageWidth = upgradeProgressBackground.rect.width;
            progressImageLevelLength = 1f / progressImageLevelsCount;

            dataLoader.Dispose();
            weaponContext.Clear();

            this.weaponId = weaponId;
            dataLoader = weaponFullDataUseCase
                .Get(weaponId)
                .Subscribe(OpenWeaponData)
                .AddTo(this);
        }

        private void OnDisable()
        {
            weaponContext.Clear();
            dataLoader.Dispose();
        }

        private void OnDestroy()
        {
            weaponContext.Clear();
            dataLoader.Dispose();
        }

        private void OpenWeaponData(WeaponFullData data)
        {
            var state = data.Weapon;
            var info = state.info;

            graphics.sprite = iconRepository.GetIcon(info.id);

            SetProgressRectLevel(upgradeProgress, state.currentLevel);
            SetProgressRectLevel(upgradeProgressBackground, info.settingsLevels.Length);
            SetTitle(info.nameLocalizationKey);
            SetDescription(info.nameLocalizationKey);
            SetStats(state.CurrentSettings);

            purchaseButtonRoot.SetActive(!state.Purchased);
            if (!state.Purchased)
            {
                purchaseButton.SetIcon(data.IsLocked ? lockedIcon : avaliableForPurchaseIcon);
                purchaseButton.SetText("");
                var purchaseText = data.IsLocked ? purchaseLockedText : purchaseAvailableText;
                purchaseText.GetLocalizedStringAsync().Completed += result =>
                {
                    var replacer = data.IsLocked ? info.availableFromLevel.ToString() : state.Cost.ToString();
                    var text = result.Result.Replace("$", replacer);
                    purchaseButton.SetText(text);
                };
                purchaseButton.Interactable(data.IsPurchasable);
                purchaseButton.onClick.RemoveAllListeners();
                purchaseButton.onClick.AddListener(TryPurchase);
            }

            upgradeButtonRoot.SetActive(state.Upgradable);
            if (state.Upgradable)
            {
                upgradeButton.Interactable(data.IsUpgradable);
                upgradeButton.SetText("");
                upgradeText.GetLocalizedStringAsync().Completed += result =>
                {
                    var text = result.Result.Replace("$", state.Cost.ToString());
                    upgradeButton.SetText(text);
                };
                upgradeButton.onClick.RemoveAllListeners();
                upgradeButton.onClick.AddListener(TryUpgrade);
            }

            setSlotButtonRoot.SetActive(state.Purchased);
            if (state.Purchased)
            {
                var slotText = data switch
                {
                    { IsPrimary: true } => primarySlotText,
                    { IsSecondary: true } => secondarySlotText,
                    _ => setSlotText
                };
                setSlotButton.SetText("");
                slotText.GetLocalizedStringAsync().Completed += result => setSlotButton.SetText(result.Result);

                setSlotButton.Interactable(data is { IsPrimary: false, IsSecondary: false });
                setSlotButton.onClick.RemoveAllListeners();
                setSlotButton.onClick.AddListener(OpenSlotSelection);
            }

            manager.Open();
        }

        private void OpenSlotSelection()
        {
            playSoundNavigator.Play(SoundType.ButtonDefault);
            setPrimarySlotButton.onClick.RemoveAllListeners();
            setPrimarySlotButton.onClick.AddListener(() => SetWeaponSlot(true));
            setSecondarySlotButton.onClick.RemoveAllListeners();
            setSecondarySlotButton.onClick.AddListener(() => SetWeaponSlot(false));
            slotSelectionModalWindowTitleText.GetLocalizedStringAsync().Completed += (result) =>
            {
                slotSelectionManager.titleText = result.Result;
                slotSelectionManager.UpdateUI();
                slotSelectionManager.Open();
            };
        }

        private void SetWeaponSlot(bool isPrimary)
        {
            playSoundNavigator.Play(SoundType.ButtonDefault);
            setWeaponSlotUseCase
                .TrySetSlot(weaponId, isPrimary)
                .Subscribe(OnSetSlotResult)
                .AddTo(weaponContext);
            slotSelectionManager.Close();
        }

        private void OnSetSlotResult(bool result)
        {
            playSoundNavigator.Play(result ? SoundType.ButtonOk : SoundType.Warning);
            ShowNotification(
                title: result ? slotSelectedNotificationTitleText : slotSelectionFailedTitleText,
                desc: result ? slotSelectedNotificationDescText : slotSelectionFailedDescText,
                icon: result ? actionSuccessIcon : actionFailureIcon
            );
        }

        private void TryUpgrade()
        {
            playSoundNavigator.Play(SoundType.ButtonDefault);
            operationsUseCase
                .TryUpgrade(weaponId)
                .Subscribe(OnUpgradeResult)
                .AddTo(this);
        }

        private void OnUpgradeResult(bool result)
        {
            playSoundNavigator.Play(result ? SoundType.Upgrade : SoundType.Warning);
            ShowNotification(
                title: result ? upgradedNotificationTitleText : upgradeFailedTitleText,
                desc: result ? upgradedNotificationDescText : upgradeFailedDescText,
                icon: result ? actionSuccessIcon : actionFailureIcon
            );
        }

        private void TryPurchase()
        {
            playSoundNavigator.Play(SoundType.ButtonDefault);
            operationsUseCase
                .TryPurchase(weaponId)
                .Subscribe(OnPurchaseResult)
                .AddTo(this);
        }

        private void OnPurchaseResult(bool result)
        {
            playSoundNavigator.Play(result ? SoundType.Purchase : SoundType.Warning);
            ShowNotification(
                title: result ? purchasedNotificationTitleText : purchaseFailedTitleText,
                desc: result ? purchasedNotificationDescText : purchaseFailedDescText,
                icon: result ? actionSuccessIcon : actionFailureIcon
            );
        }

        private void SetStats(WeaponSettings settings)
        {
            ammoAmountText.text = settings.rounds.ToString();
            rpmAmountText.text = settings.rpm.ToString();
            damageAmountText.text = settings.damage.ToString();
        }

        private void SetTitle(string localizationKey)
        {
            manager.titleText = "";
            weaponNameTable
                .GetLocalizedStringObservable(localizationKey)
                .Subscribe(result =>
                {
                    manager.titleText = result;
                    manager.UpdateUI();
                })
                .AddTo(weaponContext);
        }

        private void SetDescription(string localizationKey)
        {
            description.text = "";
            weaponNameTable
                .GetLocalizedStringObservable($"{localizationKey}_description")
                .Subscribe(result => description.text = result)
                .AddTo(weaponContext);
        }

        private void SetProgressRectLevel(RectTransform target, int level)
        {
            var progressWidth = Mathf.Clamp01(progressImageLevelLength * level) * progressImageWidth;
            target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, progressWidth);
        }

        private void ShowNotification(LocalizedString title, LocalizedString desc, Sprite icon) =>
            Observable
                .CombineLatest(
                    title.GetLocalizedStringObservable(),
                    desc.GetLocalizedStringObservable()
                )
                .Subscribe(strings => ShowNotification(strings[0], strings[1], icon))
                .AddTo(weaponContext);

        private void ShowNotification(string title, string desc, Sprite icon)
        {
            Debug.Log("ShowNotification");
            actionResultsManager.icon = icon;
            actionResultsManager.title = title;
            actionResultsManager.description = desc;
            actionResultsManager.UpdateUI(); 
            actionResultsManager.isOn = false;
            actionResultsManager.OpenNotification();
        }
    }
}