using System;
using Core.Auth.domain;
using Core.Auth.domain.model;
using UniRx;

namespace Core.Auth.data
{
    public class AlwaysLoggedInAuthRepository : IAuthRepository
    {
        public string LoginUserId => "";
        public IObservable<bool> GetLoggedInFlow() => Observable.Return(true);
        public IObservable<bool> Login(AuthData authData) => Observable.Return(true);
    }
}