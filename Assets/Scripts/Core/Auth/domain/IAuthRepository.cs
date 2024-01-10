using System;
using Core.Auth.domain.model;
using ModestTree;

namespace Core.Auth.domain
{
    public interface IAuthRepository
    {
        public IObservable<bool> GetLoggedInFlow();
        public string LoginUserId { get; }
        public IObservable<bool> Login(AuthData authData);
    }

    public static class IAuthRepositoryExtenstions
    {
        public static bool IsLoggedIn(this IAuthRepository repository) => !repository.LoginUserId.IsEmpty();
    }
}