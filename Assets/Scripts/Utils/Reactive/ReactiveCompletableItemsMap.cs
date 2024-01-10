using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace HNS.domain
{
    public class ReactiveCompletableItemsMap<TKey, TItem> where TKey : IEquatable<TKey>
    {
        private readonly ReactiveDictionary<TKey, BehaviorSubject<TItem>> items;

        public ReactiveCompletableItemsMap() => items = new ReactiveDictionary<TKey, BehaviorSubject<TItem>>();

        public ReactiveCompletableItemsMap(IDictionary<TKey, TItem> items)
        {
            var itemsDictionary = items.ToDictionary(
                kvp => kvp.Key,
                kvp => new BehaviorSubject<TItem>(kvp.Value)
            );
            this.items = new ReactiveDictionary<TKey, BehaviorSubject<TItem>>(itemsDictionary);
        }

        private BehaviorSubject<TItem> GetOrCreateSubject(TKey key, TItem defValue)
        {
            if (items.TryGetValue(key, out var subject))
                return subject;

            var createdSubject = new BehaviorSubject<TItem>(defValue);
            items[key] = createdSubject;
            return createdSubject;
        }

        private void CompleteAndRemoveUnsafe(TKey key)
        {
            var subject = items[key];
            subject.OnCompleted();
            items.Remove(key);
        }

        public IObservable<TItem> GetInFuture(TKey key)
        {
            if (items.TryGetValue(key, out var subject))
                return subject;

            return items
                .ObserveAdd()
                .Where(addEvent => addEvent.Key.Equals(key))
                .Select(addEvent => addEvent.Value)
                .Take(1)
                .Switch();
        }

        public bool Get(TKey key, out IObservable<TItem> itemFlow)
        {
            var itemPresent = items.TryGetValue(key, out var subject);
            if (!itemPresent)
            {
                itemFlow = null;
                return false;
            }

            itemFlow = subject;
            return true;
        }

        public void Set(TKey key, TItem item) => GetOrCreateSubject(key, item).OnNext(item);

        public bool Remove(TKey key)
        {
            if (!items.ContainsKey(key))
                return false;

            CompleteAndRemoveUnsafe(key);
            return true;
        }

        public void Clear()
        {
            items.Keys.ToList().ForEach(CompleteAndRemoveUnsafe);
            items.Clear();
        }
    }
}