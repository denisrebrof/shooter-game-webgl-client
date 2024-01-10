using System;

namespace Features.Balance.domain.repositories
{
    public interface IBalanceRepository
    {
        IObservable<int> GetBalanceFlow(string currencyId);
        void Add(int value, string currencyId);
        void Remove(int value, string currencyId);
    }
}