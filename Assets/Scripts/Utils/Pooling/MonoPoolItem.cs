using UnityEngine;

namespace Utils.Pooling
{
    public class MonoPoolItem: MonoBehaviour, IPoolItem
    {
        [SerializeField, HideInInspector] private GameObject goCache;

        private void Reset()
        {
            goCache = gameObject;
        }

        public void OnGetFromPool() => goCache.SetActive(true);

        public void OnReturnToPool() => goCache.SetActive(false);
    }
}