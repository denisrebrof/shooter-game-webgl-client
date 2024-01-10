using Features.Purchases.domain;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Features.Purchases.presentation
{
    public class PurchasedStateListener : MonoBehaviour
    {
        [SerializeField] private string purchaseID;
        [SerializeField] private UnityEvent onPurchased = new();
        [SerializeField] private UnityEvent onNotPurchased = new();

        [Inject] private PurchasedStateUseCase purchasedStateUseCase;

        private void Start() => purchasedStateUseCase
            .GetPurchasedState(purchaseID)
            .Select(state => state ? onPurchased : onNotPurchased)
            .Subscribe(selected => selected.Invoke())
            .AddTo(this);
    }
}