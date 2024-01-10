using System.Collections;
using Features.Balance.domain;
using Features.Balance.domain.repositories;
using Features.Currencies.data;
using UnityEngine;
using UnityEngine.UI;
using Utils.StringSelector;
using Zenject;

namespace Features.Balance.presentation
{
    public class CollectRewardView : MonoBehaviour
    {
        [Inject] private IRewardRepository rewardRepository;
        [Inject] private CollectRewardUseCase collectReward;
        [Header("Views")] [SerializeField] private Text rewardText;
        [SerializeField] private Text multiplierText;
        [SerializeField] private GameObject multiplierRoot;
        [Header("Counter")] [SerializeField] private int maxMultiplier = 2;
        [SerializeField] private int minMultiplier = 2;
        [SerializeField] private int counterCycleDuration = 0;
        [SerializeField] private int counterStartDelay = 0;
        [SerializeField] private int counterStep = 1;
        [SerializeField, StringSelector(typeof(SOCurrencyRepository))] 
        private string currencyId;

        private int multiplier = 1;

        public void Collect(bool withMultiplier)
        {
            StopAllCoroutines();
            multiplierRoot.SetActive(false);
            if (!withMultiplier)
            {
                collectReward.Collect(currencyId);
                return;
            }

            collectReward.Collect(currencyId, multiplier);
        }

        private void OnEnable()
        {
            multiplierRoot.SetActive(true);
            rewardText.text = rewardRepository.Get().ToString();
            StartCoroutine(MultiplierCoroutine());
        }

        private void OnDisable() => StopAllCoroutines();

        private IEnumerator MultiplierCoroutine()
        {
            SetMultiplier(maxMultiplier, true);
            if (counterCycleDuration <= 0)
                yield break;

            if (counterStartDelay > 0)
                yield return new WaitForSeconds(counterStartDelay);

            while (multiplier > minMultiplier)
            {
                yield return new WaitForSeconds(counterCycleDuration);
                SetMultiplier(Mathf.Max(multiplier - counterStep, minMultiplier));
            }
        }

        private void SetMultiplier(int value, bool immediate = false)
        {
            multiplierText.text = value.ToString();
            multiplier = value;
        }
    }
}