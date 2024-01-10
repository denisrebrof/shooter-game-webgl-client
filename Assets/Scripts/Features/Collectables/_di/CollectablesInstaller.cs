using Features.Collectables.data;
using UnityEngine;
using Zenject;

namespace Features.Collectables._di
{
    [CreateAssetMenu(menuName = "Installers/CollectablesInstaller")]
    public class CollectablesInstaller: ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LocalStorageCollectableRepository>().AsSingle();
        }
    }
}