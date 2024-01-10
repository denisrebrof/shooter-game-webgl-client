using Features.Purchases.domain.repositories;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.Purchases.presentation.ui
{
    public class CurrencyPurchaseItem: PurchaseItem
    {
        [Inject] private ICurrencyPurchaseRepository currencyPurchaseRepository;
        
        [SerializeField] private GameObject cost;
        [SerializeField] private Text costText;

        protected override void Setup(string purchaseId, bool purchasedState)
        {
            base.Setup(purchaseId, purchasedState);
            costText.text = currencyPurchaseRepository.GetData(purchaseId).Cost.ToString();
            cost.SetActive(!purchasedState);
        }

        public class Factory : PlaceholderFactory<CurrencyPurchaseItem> { }
    }
}