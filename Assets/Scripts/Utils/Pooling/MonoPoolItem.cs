using UnityEngine;

namespace Utils.Pooling
{
    public class MonoPoolItem: MonoBehaviour, IPoolItem
    {
        [SerializeField, HideInInspector] private GameObject goCache;
        [SerializeField, HideInInspector] protected Transform target;

        private void Reset()
        {
            goCache = gameObject;
            target = gameObject.transform;
        }

        [ContextMenu("Grab Hidden Components")]
        private void GrabHiddenComponents()
        {
            goCache = gameObject;
            target = gameObject.transform;
        }
        
        [ContextMenu("Log Hidden Components")]
        private void LogHiddenComponents()
        {
            Debug.Log($"{goCache} {target}");
        }

        public virtual void OnGetFromPool() => goCache.SetActive(true);

        public virtual void OnReturnToPool() => goCache.SetActive(false);
    }
}