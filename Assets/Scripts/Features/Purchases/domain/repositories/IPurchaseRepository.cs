using System.Collections.Generic;
using Features.Purchases.domain.model;

namespace Features.Purchases.domain.repositories
{
    public interface IPurchaseRepository
    {
        public List<Purchase> GetPurchases(string categoryId);
        public Purchase GetById(string id);
    }
}