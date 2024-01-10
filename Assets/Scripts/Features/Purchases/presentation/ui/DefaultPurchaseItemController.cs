using System;
using Features.Purchases.domain;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Purchases.presentation.ui
{
    public class DefaultPurchaseItemController : MonoBehaviour, PurchaseItem.IPurchaseItemController
    {
        [Inject] private PurchasedStateUseCase purchasedStateUseCase;
        [Inject] private ExecutePurchaseUseCase executePurchaseUseCase;

        [Inject] private IPurchaseItemSelectionAdapter selectionAdapter;

        public void OnItemClick(string purchaseId) => purchasedStateUseCase
            .GetPurchasedState(purchaseId)
            .Subscribe(purchasedState =>
                HandleItemClick(purchasedState, purchaseId)
            ).AddTo(this);

        public IObservable<bool> GetPurchasedState(string purchaseId) => purchasedStateUseCase
            .GetPurchasedState(purchaseId);

        private void HandleItemClick(bool purchasedState, string purchaseId)
        {
            if (purchasedState)
            {
                selectionAdapter.SelectItem(purchaseId);
                return;
            }

            executePurchaseUseCase.ExecutePurchase(purchaseId).Subscribe().AddTo(this);
        }

        public interface IPurchaseItemSelectionAdapter
        {
            public void SelectItem(string purchaseId);
        }
    }
}