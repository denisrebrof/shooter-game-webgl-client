using System;
using Features.Purchases.domain.model;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Features.Purchases.presentation.ui
{
    public class PurchaseItem : MonoBehaviour
    {
        [Inject] private IPurchaseItemController itemController;
        
        [SerializeField] private GameObject unavailableStub;
        [SerializeField] private GameObject availableStub;
        [SerializeField] private Text itemName;
        [SerializeField] private Text description;

        [CanBeNull] private string purchaseID;

        public void Setup(Purchase purchase)
        {
            purchaseID = purchase.Id;

            if (itemName != null)
                itemName.text = purchase.Name;
            
            if (description != null)
                description.text = purchase.Description;
            
            itemController
                .GetPurchasedState(purchase.Id)
                .Subscribe(purchased =>
                    Setup(purchase.Id, purchased)
                ).AddTo(this);
        }

        protected virtual void Setup(string purchaseId, bool purchasedState)
        {
            if(unavailableStub!=null)
                unavailableStub.SetActive(!purchasedState);
            if(availableStub!=null)
                availableStub.SetActive(purchasedState);
        }

        public void Click()
        {
            if (purchaseID == null) return;
            itemController.OnItemClick(purchaseID);
        }

        public interface IPurchaseItemController
        {
            void OnItemClick(string purchaseId);
            IObservable<bool> GetPurchasedState(string purchaseId);
        }
    }
}