using Features.Hints.data;
using Features.Hints.domain;
using Features.Hints.presentation;
using UnityEngine;
using Zenject;

namespace Features.Hints._di
{
    [CreateAssetMenu(menuName = "Installers/HintsInstaller")]
    public class HintsInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            //Repositories
            Container.Bind<ICurrentHintRepository>().To<CurrentHintInMemoryRepository>().AsSingle();

            var navigator = FindObjectOfType<HintsNavigator>();
            Container.BindInstance(navigator).AsSingle();
        }
    }
}