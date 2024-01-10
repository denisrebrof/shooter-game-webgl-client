using System;
using Core.User.domain;
using Plugins.FileIO;
using UniRx;
using Zenject;
using static Core.User.domain.ICurrentUserNameRepository;

namespace Core.User.data
{
    public class LocalUserNameRepository : ICurrentUserNameRepository
    {
        private const string DEFAULT_USERNAME = "Username";
        private const string USERNAME_KEY = "username";

        private readonly BehaviorSubject<string> userNameFlow;

        [Inject]
        public LocalUserNameRepository()
        {
            var userName = LocalStorageIO.HasKey(USERNAME_KEY)
                ? LocalStorageIO.GetString(USERNAME_KEY)
                : DEFAULT_USERNAME;
            userNameFlow = new BehaviorSubject<string>(userName);
        }

        public IObservable<string> GetUserNameFlow() => userNameFlow;

        public IObservable<UpdateUserNameResult> UpdateUserName(string newName)
        {
            LocalStorageIO.SetString(USERNAME_KEY, newName);
            userNameFlow.OnNext(newName);
            return Observable.Return(UpdateUserNameResult.Success);
        }
    }
}