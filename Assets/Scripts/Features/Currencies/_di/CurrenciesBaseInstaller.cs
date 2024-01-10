using Features.Currencies.data;
using UnityEngine;
using Zenject;

namespace Features.Currencies._di
{
    [CreateAssetMenu(menuName = "Installers/CurrenciesBaseInstaller")]
    public class CurrenciesBaseInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private SOCurrencyRepository currencyRepository;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SOCurrencyRepository>().FromInstance(currencyRepository).AsSingle();
        }
    }
}
