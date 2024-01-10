using Features.Purchases.domain.repositories;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.Purchases.presentation.ui
{
    public class PurchaseImage : MonoBehaviour
    {
        [Inject] private IPurchaseImageRepository imageRepository;
        [SerializeField] private Image target;

        public void Setup(string purchaseId) => target.sprite = imageRepository.GetImage(purchaseId);
    }
}