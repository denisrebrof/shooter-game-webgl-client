#if PLAYFAB
using PlayFab;
using PlayFab.ClientModels;
using System;
using Core.Auth.domain;
using Core.User.domain;
using UniRx;
using Zenject;
using static Core.User.domain.ICurrentUserNameRepository;

namespace Core.User.data
{
    public class PlayfabUserNameRepository : ICurrentUserNameRepository
    {
        [Inject] private IAuthRepository authRepository;
        [Inject] private UserNameLocalDataSource userNameLocalDataSource;

        public IObservable<string> GetUserNameFlow() => userNameLocalDataSource.GetUserNameFlow();

        public IObservable<UpdateUserNameResult> UpdateUserName(string newName) => authRepository
            .GetLoggedInFlow()
            .Where(loggedIn => loggedIn)
            .First()
            .Select(_ => UpdatePlayfabUserName(newName))
            .Switch();

        private IObservable<UpdateUserNameResult> UpdatePlayfabUserName(string newName) => Observable.Create(
            (IObserver<UpdateUserNameResult> observer) =>
            {
                var request = new UpdateUserTitleDisplayNameRequest
                {
                    DisplayName = newName
                };
                PlayFabClientAPI.UpdateUserTitleDisplayName(
                    request,
                    success =>
                    {
                        userNameLocalDataSource.UpdateUserName(newName);
                        observer.OnNext(UpdateUserNameResult.Success);
                        observer.OnCompleted();
                    },
                    error =>
                    {
                        var result = error.Error == PlayFabErrorCode.NameNotAvailable
                            ? UpdateUserNameResult.NotAvailable
                            : UpdateUserNameResult.Error;
                        observer.OnNext(result);
                        observer.OnCompleted();
                    }
                );
                return Disposable.Create(() => { });
            }
        );
    }
}
#endif