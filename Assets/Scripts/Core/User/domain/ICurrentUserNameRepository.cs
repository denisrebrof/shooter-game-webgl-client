using System;

namespace Core.User.domain
{
    public interface ICurrentUserNameRepository
    {
        IObservable<string> GetUserNameFlow();
        IObservable<UpdateUserNameResult> UpdateUserName(string newName);
        
        public enum UpdateUserNameResult
        {
            Success,
            NotAvailable,
            Error
        }
    }
}