using System;
using System.Collections.Generic;
using Features.Purchases.domain.model;
using Features.Purchases.domain.repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Purchases.data
{
    public class CurrencyPurchaseRepository : ICurrencyPurchaseRepository
    {
        [Inject] private IPurchaseEntitiesDao entitiesDao;
        [Inject] private ISavedPurchasedStateDao stateDao;

        private readonly Dictionary<string, ReactiveProperty<bool>> purchasedStateProcessors = new();

        public CurrencyPurchaseData GetData(string purchaseId)
        {
            var entity = entitiesDao.FindById(purchaseId);
            var cost = Mathf.Max(entity.currencyCost, 0);
            return new CurrencyPurchaseData(cost, entity.currencyId);
        }

        public void SetPurchased(string purchaseId)
        {
            stateDao.SetPurchasedState(purchaseId);
            if (!purchasedStateProcessors.ContainsKey(purchaseId)) return;
            var processor = purchasedStateProcessors[purchaseId];
            processor.Value = true;
            processor.Dispose();
            purchasedStateProcessors.Remove(purchaseId);
        }

        public IObservable<bool> GetPurchasedState(string purchaseId)
        {
            if (purchasedStateProcessors.ContainsKey(purchaseId))
                return purchasedStateProcessors[purchaseId];

            var purchasedState = stateDao.GetPurchasedState(purchaseId);
            if (purchasedState)
                return Observable.Return(true);

            var processor = new ReactiveProperty<bool>(false);
            purchasedStateProcessors[purchaseId] = processor;
            return processor;
        }
    }
}