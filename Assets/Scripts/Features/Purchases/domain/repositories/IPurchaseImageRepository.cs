using UnityEngine;

namespace Features.Purchases.domain.repositories
{
    public interface IPurchaseImageRepository
    {
        public Sprite GetImage(string purchaseId);
    }
}