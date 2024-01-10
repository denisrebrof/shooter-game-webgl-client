using System;
using System.Collections.Generic;
using Core.User.domain;
using UniRx;
using UnityEngine;
using Zenject;
using static Core.User.domain.ICurrentUserNameRepository;
using Random = UnityEngine.Random;

namespace Core.User.presentation
{
    public class UserNameSetup : MonoBehaviour
    {
        [SerializeField] private List<string> defaultUserNamePrefixes = new();
        [SerializeField] private int maxRetryTimes = 3;
        [Inject] private ICurrentUserNameRepository currentUserNameRepository;

        private string NewUsername
        {
            get
            {
                var nameIndex = Random.Range(0, defaultUserNamePrefixes.Count);
                return defaultUserNamePrefixes[nameIndex] + "-" + Guid.NewGuid().ToString().Substring(0, 8);
            }
        }

        private void Start() => currentUserNameRepository
            .GetUserNameFlow()
            .Where(string.IsNullOrWhiteSpace)
            .Subscribe(_ => SetRandomUserName())
            .AddTo(this);

        private void SetRandomUserName()
        {
            if (defaultUserNamePrefixes.Count == 0)
                return;

            currentUserNameRepository
                .UpdateUserName(NewUsername)
                .Subscribe(result => OnUsernameUpdated(result))
                .AddTo(this);
        }

        private void OnUsernameUpdated(UpdateUserNameResult result, int tryNum = 1)
        {
            if (result != UpdateUserNameResult.NotAvailable || tryNum > maxRetryTimes)
                return;

            //Another try
            currentUserNameRepository
                .UpdateUserName(NewUsername)
                .Subscribe(newResult => OnUsernameUpdated(newResult, tryNum + 1))
                .AddTo(this);
        }
    }
}