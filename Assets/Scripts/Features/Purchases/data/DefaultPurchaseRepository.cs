using System.Collections.Generic;
using System.Linq;
using Features.Purchases.data.model;
using Features.Purchases.domain.model;
using Features.Purchases.domain.repositories;
using UnityEngine;
using Zenject;

namespace Features.Purchases.data
{
    public class DefaultPurchaseRepository : IPurchaseRepository, IPurchaseImageRepository
    {
        [Inject] private IPurchaseEntitiesDao entitiesDao;
        [Inject] private PurchaseEntityConverter converter;

        public List<Purchase> GetPurchases(string categoryId = PurchaseCategories.DefaultCategory) => entitiesDao
            .GetEntities(categoryId)
            .Select(converter.GetPurchaseFromEntity)
            .ToList();

        public Purchase GetById(string id)
        {
            var purchaseEntity = entitiesDao.FindById(id);
            return converter.GetPurchaseFromEntity(purchaseEntity);
        }

        public Sprite GetImage(string purchaseId)
        {
            throw new System.NotImplementedException();
        }
    }
}