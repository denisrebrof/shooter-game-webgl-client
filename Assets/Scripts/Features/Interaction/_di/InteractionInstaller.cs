using Features.Interaction.data;
using Features.Interaction.domain;
using UnityEngine;
using Zenject;

namespace Features.Interaction._di
{
    [CreateAssetMenu(fileName = "InteractionInstaller", menuName = "Installers/InteractionInstaller")]
    public class InteractionInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            var repository = FindObjectOfType<RaycastInteractableRepository>();
            Container.Bind<ISelectedInteractableRepository>().FromInstance(repository).AsSingle();
        }
    }
}