using Core.Sound.presentation;
using Features.Balance.domain;
using Features.Balance.domain.repositories;
using UnityEngine;
using Zenject;

namespace Features.Balance.presentation
{
    public class AddBalanceNavigator : MonoBehaviour
    {
        [SerializeField] private AudioClip collectSound;
        [Inject] private PlaySoundNavigator playSoundNavigator;
        [Inject] private IBalanceRepository balanceRepository;

        public void AddBalance(int amount, string currencyId)
        {
            if (collectSound != null)
                playSoundNavigator.Play(collectSound);
            
            balanceRepository?.Add(amount, currencyId);
        }
    }
}