#if PLAYFAB
using System;
using Core.Auth.domain;
using PlayFab;
using PlayFab.ClientModels;
using Plugins.FileIO;

using UniRx;
using UnityEngine;

namespace Core.Auth.data
{
    public class PlayfabAuthRepository : IAuthRepository
    {
        private readonly ReactiveProperty<bool> loggedInFlow = new(false);
        private const string UserIDKey = "PLAYFAB_USER_ID_KEY";

        private static string LoginUserId
        {
            get
            {
                if (LocalStorageIO.HasKey(UserIDKey))
                    return LocalStorageIO.GetString(UserIDKey);

                var userId = Guid.NewGuid().ToString();
                LocalStorageIO.SetString(UserIDKey, userId);
                LocalStorageIO.Save();

                return userId;
            }
            set => LocalStorageIO.SetString(UserIDKey, value);
        }

        private string userId = "";

        public IObservable<bool> GetLoggedInFlow() => loggedInFlow.DistinctUntilChanged();

        string IAuthRepository.LoginUserId => userId;

        void IAuthRepository.Login(Action onSuccess, Action onFailed)
        {
            PlayFabClientAPI.LoginWithCustomID(
                new LoginWithCustomIDRequest
                {
                    CustomId = LoginUserId,
                    CreateAccount = true
                },
                result =>
                {
                    Debug.Log("Login completed.");
                    userId = result.PlayFabId;
                    loggedInFlow.Value = true;
                    onSuccess();
                },
                error =>
                {
                    Debug.LogError("Login failed.");
                    Debug.LogError(error.GenerateErrorReport());
                    onFailed();
                }
            );
        }
    }
}
#endif