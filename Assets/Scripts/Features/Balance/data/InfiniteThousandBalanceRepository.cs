using System;
using Features.Balance.domain;
using Features.Balance.domain.repositories;
using UniRx;

namespace Features.Balance.data
{
    public class InfiniteThousandBalanceRepository: IBalanceRepository
    {
        public IObservable<int> GetBalanceFlow(string currencyId)
        {
            return Observable.Return(1000);
        }

        public void Add(int value, string currencyId)
        {
            //do nothing
        }

        public void Remove(int value, string currencyId)
        {
            //do nothing
        }
    }
}