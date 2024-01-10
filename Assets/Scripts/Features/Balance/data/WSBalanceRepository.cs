using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Features.Balance.domain.repositories;
using UniRx;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Features.Balance.data
{
    public class WSBalanceRepository : IBalanceRepository
    {
        private static BalanceState DefaultState = new BalanceState
        {
            currencies = new List<string>(),
            amounts = new List<long>(),
        };

        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        private readonly ReactiveProperty<BalanceState> balanceState = new(DefaultState);

        private IDisposable balanceUpdatesHandler;

        public IObservable<int> GetBalanceFlow(string currencyId)
        {
            if (balanceUpdatesHandler != null)
                return GetBalance(currencyId);

            balanceUpdatesHandler = commandsUseCase
                .Subscribe<BalanceState>(Commands.Balance)
                .Subscribe(state => balanceState.Value = state);

            return GetBalance(currencyId);
        }

        private IObservable<int> GetBalance(string currencyId) => balanceState
            .Select(state => state.GetAmount(currencyId))
            .DistinctUntilChanged();

        public void Add(int value, string currencyId)
        {
            //do nothing
        }

        public void Remove(int value, string currencyId)
        {
            //do nothing
        }

        [Serializable]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private struct BalanceState
        {
            public List<string> currencies;
            public List<long> amounts;

            public int GetAmount(string currencyId)
            {
                if (!currencies.Contains(currencyId))
                    return 0;

                var index = currencies.IndexOf(currencyId);
                return (int)amounts[index];
            }
        }
    }
}