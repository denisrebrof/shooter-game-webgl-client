using System;
using System.Collections.Generic;
using Core.Localization;
using Core.User.domain;
using JetBrains.Annotations;
using Michsky.MUIP;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Zenject;

namespace Shooter.presentation.UI.Lobby
{
    public class ChangeNickModalWindow : MonoBehaviour
    {
        [SerializeField] private NotificationManager notificationManager;
        [SerializeField] private LocalizedString notificationTitleText;
        [SerializeField] private LocalizedString notificationDescText;
        [SerializeField] private ModalWindowManager manager;
        [SerializeField] private LocalizedString titleText;
        [SerializeField] private TMP_InputField editNameField;
        [SerializeField] private TMP_Text errorText;
        [SerializeField] private GameObject errorMark;
        
        [SerializeField] private UnityEvent<bool> ConfirmAvailable;

        [Inject] private ICurrentUserNameRepository currentUserNameRepository;
        [Inject] private ValidateUserNameUseCase validateUserNameUseCase;
        [Inject] private ILanguageProvider languageProvider;

        [CanBeNull] private IDisposable setupDisposable;

        private bool IsRussian => LocalizationSettings.SelectedLocale.Identifier.Code == "ru";

        private string UnableToSetUsernameErrorText => IsRussian
            ? unableToSetUsernameErrorTextRu
            : unableToSetUsernameErrorTextEn;

        private const string unableToSetUsernameErrorTextRu = "Ошибка :(";
        private const string unableToSetUsernameErrorTextEn = "Error while updating name :(";

        private string UnableToSetUsernameNotAvailableText => IsRussian
            ? unableToSetUsernameNotAvailableTextRu
            : unableToSetUsernameNotAvailableTextEn;

        private const string unableToSetUsernameNotAvailableTextRu = "Имя недоступно";
        private const string unableToSetUsernameNotAvailableTextEn = "Username not available, try another one";

        private Dictionary<ValidateUserNameUseCase.UserNameValidState, string> ErrorTexts =>
            IsRussian ? errorTextsRu : errorTextsEn;

        private readonly Dictionary<ValidateUserNameUseCase.UserNameValidState, string> errorTextsEn = new()
        {
            [ValidateUserNameUseCase.UserNameValidState.Valid] = "",
            [ValidateUserNameUseCase.UserNameValidState.TooShort] = "Name is too short!",
            [ValidateUserNameUseCase.UserNameValidState.TooLong] = "Name is too long!",
            [ValidateUserNameUseCase.UserNameValidState.IsEmpty] = "Name is empty!",
            [ValidateUserNameUseCase.UserNameValidState.ContainsInvalidCharacters] = "Name contains invalid characters!"
        };

        private readonly Dictionary<ValidateUserNameUseCase.UserNameValidState, string> errorTextsRu = new()
        {
            [ValidateUserNameUseCase.UserNameValidState.Valid] = "",
            [ValidateUserNameUseCase.UserNameValidState.TooShort] = "Слишком короткое!",
            [ValidateUserNameUseCase.UserNameValidState.TooLong] = "Слишком длинное!",
            [ValidateUserNameUseCase.UserNameValidState.IsEmpty] = "Имя пустое!",
            [ValidateUserNameUseCase.UserNameValidState.ContainsInvalidCharacters] = "Недопустимые символы!"
        };

        private void OnEnable()
        {
            manager.titleText = titleText.GetLocalizedString();
            manager.UpdateUI();
            ConfirmAvailable.Invoke(false);
            errorMark.SetActive(false);
            setupDisposable = currentUserNameRepository.GetUserNameFlow().First().Subscribe(Setup);
        }

        private void OnDisable() => setupDisposable?.Dispose();

        public void SubmitUserName()
        {
            var userName = editNameField.text;
            if (validateUserNameUseCase.Validate(userName) != ValidateUserNameUseCase.UserNameValidState.Valid)
                return;

            ConfirmAvailable.Invoke(false);
            currentUserNameRepository.UpdateUserName(userName).Subscribe(OnUsernameUpdated).AddTo(this);
        }

        private void Setup(string username)
        {
            editNameField.text = username;
            editNameField.onValueChanged.RemoveAllListeners();
            editNameField.onValueChanged.AddListener(OnNameChanged);
        }

        private void OnUsernameUpdated(ICurrentUserNameRepository.UpdateUserNameResult result)
        {
            if (result != ICurrentUserNameRepository.UpdateUserNameResult.Success)
            {
                errorText.text = result == ICurrentUserNameRepository.UpdateUserNameResult.NotAvailable
                    ? UnableToSetUsernameNotAvailableText
                    : UnableToSetUsernameErrorText;
                errorMark.SetActive(true);
                errorText.enabled = true;
                return;
            }

            notificationManager.title = notificationTitleText.GetLocalizedString();
            notificationManager.description = notificationDescText.GetLocalizedString();
            notificationManager.UpdateUI();
            notificationManager.OpenNotification();
            manager.CloseWindow();
        }

        private void OnNameChanged(string newValue)
        {
            var state = validateUserNameUseCase.Validate(newValue);
            errorText.text = ErrorTexts[state];
            var isValid = state == ValidateUserNameUseCase.UserNameValidState.Valid;
            errorText.enabled = !isValid;
            errorMark.SetActive(!isValid);
            ConfirmAvailable.Invoke(isValid);
        }
    }
}