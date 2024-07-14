using System;
using System.Collections.Generic;
using Features.Rating.domain;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Rating.presentation
{
    public class RatingScreen : MonoBehaviour
    {
        [Inject] private RatingUseCase useCase;
        [Inject] private RatingItemPool pool;

        [SerializeField] private GameObject loader;
        [SerializeField] private RectTransform root;

        private readonly List<RatingItemView> items = new();

        private IDisposable request = Disposable.Empty;

        private void OnEnable() => Refresh();

        private void OnDisable() => request.Dispose();

        public void Refresh()
        {
            loader.SetActive(true);
            request.Dispose();
            request = useCase.GetRating().Subscribe(SetupRating);
        }

        private void SetupRating(List<RatingData> data)
        {
            for (var i = 0; i < data.Count; i++)
            {
                var view = GetView(i);
                var dataItem = data[i];
                view.Setup(
                    dataItem.username,
                    dataItem.rating,
                    dataItem.position,
                    dataItem.isMine
                );
            }

            while (items.Count > data.Count)
            {
                var lastItemIndex = items.Count - 1;
                var nextReturnItem = items[lastItemIndex];
                pool.Return(nextReturnItem);
                items.RemoveAt(lastItemIndex);
            }

            loader.SetActive(false);
        }

        private RatingItemView GetView(int index)
        {
            if (items.Count > index)
                return items[index];

            var item = pool.Pop();
            items.Add(item);

            var itemTransform = item.transform;
            itemTransform.SetParent(root);
            itemTransform.localScale = Vector3.one;
            itemTransform.SetAsFirstSibling();

            return item;
        }
    }
}