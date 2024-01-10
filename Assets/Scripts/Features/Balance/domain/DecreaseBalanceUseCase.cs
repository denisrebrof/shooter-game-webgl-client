using System;
using Features.Balance.domain.repositories;
using UniRx;
using Zenject;

namespace Features.Balance.domain
{
    public class DecreaseBalanceUseCase
    {
        [Inject] private IBalanceRepository repository;

        public IObservable<bool> GetCanDecreaseFlow(int amount, string currencyId)
        {
            return repository
                .GetBalanceFlow(currencyId)
                .Select(balance => balance >= amount);
        }

        public IObservable<DecreaseBalanceResult> Decrease(
            int amount,
            string currencyId
        ) => GetCanDecreaseFlow(amount, currencyId)
            .Take(1)
            .Select(canDecrease =>
                DecreaseBalance(canDecrease, amount, currencyId)
            );

        private DecreaseBalanceResult DecreaseBalance(bool canDecrease, int amount, string currencyId)
        {
            if (!canDecrease)
                return DecreaseBalanceResult.LowBalance;

            repository.Remove(amount, currencyId);
            return DecreaseBalanceResult.Success;
        }

        public enum DecreaseBalanceResult
        {
            Success,
            LowBalance,
            UnexpectedFailure
        }
    }
}