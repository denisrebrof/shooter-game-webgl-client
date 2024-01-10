using Core.Auth.domain;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Auth.presentation
{
    public class AuthStub : MonoBehaviour
    {
        [Inject] private IAuthRepository authRepository;

        private void Start() => authRepository
            .GetLoggedInFlow()
            .Subscribe(logged =>
                gameObject.SetActive(!logged)
            ).AddTo(this);
    }
}