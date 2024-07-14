using System;
using System.Collections.Generic;
using Core.Localization;
using Core.Sound.presentation;
using Core.User.domain;
using JetBrains.Annotations;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;
using static Core.User.domain.ICurrentUserNameRepository;
using static Core.User.domain.ValidateUserNameUseCase;

namespace Core.User.presentation
{
    public class EditableUserName : MonoBehaviour
    {
        [SerializeField] private Button confirmButton;
        [SerializeField] private TMP_InputField editNameField;
        [SerializeField] private TMP_Text errorText;
        [SerializeField] private GameObject errorMark;
        [SerializeField] private UnityEvent onSuccessfulChangeUserName;

        [Inject] private PlaySoundNavigator playSoundNavigator;
        [Inject] private ICurrentUserNameRepository currentUserNameRepository;
        [Inject] private ValidateUserNameUseCase validateUserNameUseCase;
        [Inject] private ILanguageProvider languageProvider;

        private Language language;

        [CanBeNull] private IDisposable setupDisposable;

        private string UnableToSetUsernameErrorText => language == Language.Russian
            ? unableToSetUsernameErrorTextRu
            : unableToSetUsernameErrorTextEn;

        private const string unableToSetUsernameErrorTextRu = "Ошибка :(";
        private const string unableToSetUsernameErrorTextEn = "Error while updating name :(";

        private string UnableToSetUsernameNotAvailableText => language == Language.Russian
            ? unableToSetUsernameNotAvailableTextRu
            : unableToSetUsernameNotAvailableTextEn;

        private const string unableToSetUsernameNotAvailableTextRu = "Имя недоступно";
        private const string unableToSetUsernameNotAvailableTextEn = "Username not available, try another one";

        private Dictionary<UserNameValidState, string> ErrorTexts =>
            language == Language.Russian ? errorTextsRu : errorTextsEn;

        private readonly Dictionary<UserNameValidState, string> errorTextsEn = new()
        {
            [UserNameValidState.Valid] = "",
            [UserNameValidState.TooShort] = "Name is too short!",
            [UserNameValidState.TooLong] = "Name is too long!",
            [UserNameValidState.IsEmpty] = "Name is empty!",
            [UserNameValidState.ContainsInvalidCharacters] = "Name contains invalid characters!"
        };

        private readonly Dictionary<UserNameValidState, string> errorTextsRu = new()
        {
            [UserNameValidState.Valid] = "",
            [UserNameValidState.TooShort] = "Слишком короткое!",
            [UserNameValidState.TooLong] = "Слишком длинное!",
            [UserNameValidState.IsEmpty] = "Имя пустое!",
            [UserNameValidState.ContainsInvalidCharacters] = "Недопустимые символы!"
        };

        private void OnEnable()
        {
            language = languageProvider.GetCurrentLanguage();
            confirmButton.interactable = false;
            errorMark.SetActive(false);
            setupDisposable = currentUserNameRepository.GetUserNameFlow().First().Subscribe(Setup);
        }

        private void OnDisable() => setupDisposable?.Dispose();

        private void Setup(string username)
        {
            editNameField.text = username;
            editNameField.onValueChanged.RemoveAllListeners();
            editNameField.onValueChanged.AddListener(OnNameChanged);
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(SubmitUserName);
        }

        private void SubmitUserName()
        {
            var userName = editNameField.text;
            var userNameInvalid = validateUserNameUseCase.Validate(userName) != UserNameValidState.Valid;
            playSoundNavigator.Play(userNameInvalid ? SoundType.Warning : SoundType.ButtonOk);
            if (userNameInvalid)
                return;

            confirmButton.interactable = false;
            currentUserNameRepository.UpdateUserName(userName).Subscribe(OnUsernameUpdated).AddTo(this);
        }

        private void OnUsernameUpdated(UpdateUserNameResult result)
        {
            if (result != UpdateUserNameResult.Success)
            {
                errorText.text = result == UpdateUserNameResult.NotAvailable
                    ? UnableToSetUsernameNotAvailableText
                    : UnableToSetUsernameErrorText;
                errorMark.SetActive(true);
                errorText.enabled = true;
                return;
            }

            onSuccessfulChangeUserName?.Invoke();
        }

        private void OnNameChanged(string newValue)
        {
            var state = validateUserNameUseCase.Validate(newValue);
            errorText.text = ErrorTexts[state];
            var isValid = state == UserNameValidState.Valid;
            errorText.enabled = !isValid;
            errorMark.SetActive(!isValid);
            confirmButton.interactable = isValid;
        }
    }
}