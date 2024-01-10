using Features.Balance.domain.repositories;
using Features.Currencies.data;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.StringSelector;
using Zenject;

namespace Features.Balance.presentation.ui
{
    public class ReactiveBalanceText: MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField, StringSelector(typeof(SOCurrencyRepository))] 
        private string currencyId;
        [Inject] private IBalanceRepository balanceRepository;
        [SerializeField] private UnityEvent onUpdateText;

        private void Awake()
        {
            if (text != null) return;
            text = GetComponent<Text>();
        }

        private void Start() => balanceRepository
            .GetBalanceFlow(currencyId)
            .Subscribe(UpdateBalance)
            .AddTo(this);

        private void UpdateBalance(int balance)
        {
            text.text = balance.ToString();
            onUpdateText.Invoke();
        }
    }
}