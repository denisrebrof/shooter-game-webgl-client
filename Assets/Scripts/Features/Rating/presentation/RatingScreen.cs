using System;
using System.Collections.Generic;
using Features.Rating.domain;
using Shooter.presentation.UI.Rating;
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

        private List<RatingItemView> ratingViews = new();

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

            while (ratingViews.Count > data.Count)
            {
                var lastItemIndex = ratingViews.Count - 1;
                var nextReturnItem = ratingViews[lastItemIndex];
                pool.Return(nextReturnItem);
                ratingViews.RemoveAt(lastItemIndex);
            }

            loader.SetActive(false);
        }

        private RatingItemView GetView(int index)
        {
            if (ratingViews.Count > index)
                return ratingViews[index];

            var item = pool.Pop();
            ratingViews.Add(item);

            var itemTransform = item.transform;
            itemTransform.SetParent(root);
            itemTransform.localScale = Vector3.one;
            itemTransform.SetAsFirstSibling();

            return item;
        }
    }
}