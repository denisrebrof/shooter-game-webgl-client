using Features.Currencies.data;
using UnityEngine;
using Utils.StringSelector;
using Zenject;

namespace Features.Balance.presentation
{
    public class AddBalanceHandler : MonoBehaviour
    {
        [SerializeField] private int amount;
        [SerializeField, StringSelector(typeof(SOCurrencyRepository))] 
        private string currencyId;
        [Inject] private AddBalanceNavigator balanceNavigator;

        public void AddBalance()
        {
            if (balanceNavigator == null) return;
            balanceNavigator.AddBalance(amount, currencyId);
        }
    }
}