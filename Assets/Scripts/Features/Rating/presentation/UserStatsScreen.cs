using System;
using Features.Rating.domain;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Rating.presentation
{
    public class UserStatsScreen : MonoBehaviour
    {
        [Inject] private RatingUseCase useCase;

        [SerializeField] private GameObject root;
        [SerializeField] private TMP_Text kills;
        [SerializeField] private TMP_Text death;
        [SerializeField] private TMP_Text kd;

        private IDisposable request = Disposable.Empty;

        private void OnEnable() => Refresh();

        private void OnDisable() => request.Dispose();

        public void Refresh()
        {
            request.Dispose();
            request = useCase.GetStats().Subscribe(Setup);
        }

        private void Setup(UserStatsData data)
        {
            kills.text = data.kills.ToString();
            death.text = data.death.ToString();
            var kdValue = data.death > 0 ? ((float)data.kills) / data.death : 1f;
            kd.text = Mathf.Min(9999f, kdValue).ToString("0.00");
            root.SetActive(true);
        }
    }
}