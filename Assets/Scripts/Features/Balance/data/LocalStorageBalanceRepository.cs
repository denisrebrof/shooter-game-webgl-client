using System;
using Features.Balance.domain;
using Features.Balance.domain.repositories;
using Plugins.FileIO;
using UniRx;
using UnityEngine;
using Utils.Reactive;

namespace Features.Balance.data
{
    public class LocalStorageBalanceRepository : IBalanceRepository
    {
        private const string PREFS_KEY_PREFIX = "Balance_";

        private readonly ReactiveDictionary<string, int> balanceFlowMap = new();

        public IObservable<int> GetBalanceFlow(string currencyId)
        {
            balanceFlowMap[currencyId] = GetBalanceValue(currencyId);
            return balanceFlowMap.GetItemFlow(currencyId);
        }

        public void Add(int value, string currencyId)
        {
            var balance = GetBalanceValue(currencyId) + value;
            LocalStorageIO.SetInt(PREFS_KEY_PREFIX + currencyId, balance);
            LocalStorageIO.Save();
            try
            {
                balanceFlowMap[currencyId] = balance;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Remove(int value, string currencyId)
        {
            var removeResult = GetBalanceValue(currencyId) - value;
            var balance = Mathf.Max(0, removeResult);
            LocalStorageIO.SetInt(PREFS_KEY_PREFIX + currencyId, balance);
            LocalStorageIO.Save();
            balanceFlowMap[currencyId] = balance;
        }

        private static int GetBalanceValue(string currencyId) => LocalStorageIO
            .GetInt(PREFS_KEY_PREFIX + currencyId, 0);
    }
}