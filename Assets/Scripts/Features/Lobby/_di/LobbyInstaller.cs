using Features.Lobby.domain;
using UnityEngine;
using Zenject;

namespace Features.Lobby._di
{
    [CreateAssetMenu(menuName = "Installers/LobbyInstaller")]
    public class LobbyInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LobbyUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<LobbyGamesUseCase>().AsSingle();
        }
    }
}