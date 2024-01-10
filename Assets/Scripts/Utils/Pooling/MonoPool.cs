using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.Pooling
{
    public class MonoPool<T> : MonoBehaviour, IPool<T> where T : MonoBehaviour, IPoolItem
    {
        [SerializeField] private Transform root;
        [SerializeField] private int poolSize;
        [SerializeField] private T itemPrefab;

        [SerializeField] private List<T> items;

        private void Awake() => FillInitialPool();

        private void FillInitialPool()
        {
            while (items.Count < poolSize)
            {
                var spawnedItem = CreatePoolItem();
                items.Add(spawnedItem);
            }
        }

        public T Pop()
        {
            T item;
            if (items.Count < 1)
            {
                item = CreatePoolItem();
                items.Add(item);
            }
            else
            {
                item = items[0];
                items.RemoveAt(0);
            }

            item.OnGetFromPool();
            return item;
        }

        public void Return(T item)
        {
            item.OnReturnToPool();
            items.Add(item);
        }

        private T CreatePoolItem()
        {
            var spawnedItem = Instantiate(itemPrefab, root);
            spawnedItem.OnReturnToPool();
            return spawnedItem;
        }

        [ContextMenu("Generate")]
        protected void Generate()
        {
            while (root.childCount > 0)
            {
                var existingChild = root.GetChild(0).gameObject;
                DestroyImmediate(existingChild);
            }

            items = new List<T>();
            for (var i = 0; i < poolSize; i++)
            {
                var spawnedItem = CreatePoolItem();
                DestroyAutoInjector(spawnedItem.gameObject);
                items.Add(spawnedItem);
            }
        }

        private void DestroyAutoInjector(GameObject go) => go
            .GetComponents<MonoBehaviour>()
            .ToList()
            .Where(c => c.GetType().Name.Contains("ZenAutoInject"))
            .ToList()
            .ForEach(DestroyImmediate);
    }

    public interface IPool<T> where T : IPoolItem
    {
        public T Pop();
        public void Return(T item);
    }

    public interface IPoolItem
    {
        public void OnGetFromPool();
        public void OnReturnToPool();
    }
}