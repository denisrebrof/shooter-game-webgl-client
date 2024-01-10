using Features.Purchases.data.model;
using Features.Purchases.domain.model;
using Features.Purchases.domain.repositories;
using UnityEngine;
using Zenject;

namespace Features.Purchases.presentation.ui
{
    public class PurchaseList : MonoBehaviour
    {
        [SerializeField] private Transform listRoot;
        [SerializeField] private string category = PurchaseCategories.DefaultCategory;
        [Inject] private IPurchaseItemFactory purchaseItemFactory;
        [Inject] private IPurchaseRepository purchasesRepository;

        private void Awake()
        {
            if (listRoot == null)
                listRoot = transform;

            purchasesRepository.GetPurchases(category).ForEach(CreateItem);
        }

        private void CreateItem(Purchase purchase)
        {
            var item = purchaseItemFactory.Create(purchase.Type);
            item.transform.SetParent(listRoot);
            item.Setup(purchase);
        }
    }
}