using Features.Balance.presentation;
using Features.Collectables.domain;
using Features.Currencies.data;
using ModestTree;
using UnityEngine;
using Utils.AutoId;
using Utils.PlayerTrigger;
using Utils.StringSelector;
using Zenject;

namespace Features.Collectables.presentation
{
    public class CollectableCurrency : PlayerTriggerBase
    {
        [Inject] private AddBalanceNavigator addBalanceNavigator;
        [Inject] private ICollectableRepository collectableRepository;

        [SerializeField, AutoId] private string collectableId = "";
        [SerializeField] private int amount = 1;
        [SerializeField, StringSelector(typeof(SOCurrencyRepository))] 
        private string currencyId;
        [SerializeField] private ParticleSystem collectParticles;
        [SerializeField] private Animator animator;
        [SerializeField] private string trigger = "collect";

        private void Start()
        {
            if (collectableId.IsEmpty() || !collectableRepository.IsCollected(collectableId))
                return;

            gameObject.SetActive(false);
        }

        protected override void OnPlayerEntersTrigger()
        {
            if (animator != null)
                animator.SetTrigger(trigger);
            
            if (collectParticles != null)
                collectParticles.Play();
            
            addBalanceNavigator.AddBalance(amount, currencyId);
            if (collectableId.IsEmpty())
                return;

            collectableRepository.Collect(collectableId);
        }

        protected override void OnPlayerExitTrigger()
        {
            //Do nothing
        }
    }
}