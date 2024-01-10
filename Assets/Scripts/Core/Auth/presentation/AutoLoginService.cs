using System;
using Core.Auth.domain;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Auth.presentation
{
    public class AutoLoginService : IInitializable, IDisposable
    {
        [Inject] private AuthDataNavigator authDataNavigator;
        [Inject] private IAuthRepository authRepository;

        private IDisposable disposable;

        public void Initialize()
        {
            disposable = authRepository
                .GetLoggedInFlow()
                .Where(loggedIn => !loggedIn)
                .Select(_ => authDataNavigator.RequestAuthData())
                .Switch()
                .Select(data => authRepository.Login(data))
                .Switch()
                .Subscribe(loginResult =>
                    Debug.Log($"Login result: {loginResult}")
                );
        }

        public void Dispose() => disposable?.Dispose();
    }
}