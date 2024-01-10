using System;
using Plugins.FileIO;
using UniRx;
using Zenject;

namespace Core.User.data
{
    public class UserNameLocalDataSource
    {
        private readonly ReactiveProperty<string> userNameFlow;

        private const string UserNameKey = "USER_NAME_KEY";

        private string UserName
        {
            get => LocalStorageIO.HasKey(UserNameKey) ? LocalStorageIO.GetString(UserNameKey) : "";
            set
            {
                LocalStorageIO.SetString(UserNameKey, value);
                userNameFlow.Value = value;
                LocalStorageIO.Save();
            }
        }

        [Inject]
        public UserNameLocalDataSource() => userNameFlow = new ReactiveProperty<string>(UserName);

        public IObservable<string> GetUserNameFlow() => userNameFlow;

        public void UpdateUserName(string newName) => UserName = newName;
    }
}