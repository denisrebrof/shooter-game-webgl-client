using System.Collections;
using Features.Balance.domain.repositories;
using Features.Collectables.domain;
using Features.Currencies.data;
using UnityEngine;
using Utils.AutoId;
using Utils.StringSelector;
using Zenject;

namespace Features.Collectables.presentation
{
    public class CollectableChest : MonoBehaviour
    {
        [Inject] private ICollectableRepository collectableRepository;
        [Inject] private IBalanceRepository balanceRepository;

        [SerializeField] private int reward = 100;
        
        [SerializeField, StringSelector(typeof(SOCurrencyRepository))] 
        private string currencyId;
        
        [SerializeField] private GameObject unOpened;
        [SerializeField] private GameObject opened;
        [SerializeField] private ParticleSystem openParticles;
        [SerializeField] private ParticleSystem disappearParticles;
        
        [SerializeField] private float openDelay = 0.5f;
        [SerializeField] private float lookDelay = 1f;
        [SerializeField] private float disappearDelay = 0.5f;
        [SerializeField] private float destroyDelay = 0.5f;
        
        [SerializeField, AutoId] private string collectableId = "";

        private void Awake()
        {
            if (!collectableRepository.IsCollected(collectableId)) return;
            gameObject.SetActive(false);
        }

        public void Collect() => StartCoroutine(CollectCoroutine());

        private IEnumerator CollectCoroutine()
        {
            openParticles.Play();
            yield return new WaitForSeconds(openDelay);
            unOpened.SetActive(false);
            opened.SetActive(true);
            collectableRepository.Collect(collectableId);
            balanceRepository.Add(reward, currencyId);
            yield return new WaitForSeconds(lookDelay);
            disappearParticles.Play();
            yield return new WaitForSeconds(disappearDelay);
            opened.SetActive(false);
            yield return new WaitForSeconds(destroyDelay);
            gameObject.SetActive(false);
        }
    }
}