using System.Collections.Generic;
using System.Linq;
using Features.Purchases.data.model;
using UnityEngine;

namespace Features.Purchases.data.dao
{
    [CreateAssetMenu(menuName = "Purchases/Purchase Entities Dao")]
    public class SOPurchaseEntitiesDao: ScriptableObject, IPurchaseEntitiesDao
    {
        [SerializeField] private List<PurchaseEntity> entities = new();
        public List<PurchaseEntity> GetEntities(string categoryId) => entities;
        public PurchaseEntity FindById(string id) => entities.First(entity => entity.id == id);
    }
}