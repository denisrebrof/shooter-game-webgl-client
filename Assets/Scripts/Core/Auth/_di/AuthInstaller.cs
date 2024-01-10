using Core.Auth.data;
using Core.Auth.domain;
using Core.Auth.presentation;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Zenject;

namespace Core.Auth._di
{
    [CreateAssetMenu(menuName = "Installers/AuthInstaller")]
    public class AuthInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAuthRepository>()
#if PLAYFAB
            .To<PlayfabAuthRepository>()
#endif
                // .To<AlwaysLoggedInAuthRepository>()
                .To<WSAuthRepository>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<LocalStoragePlayerIdRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<LocalPlayerIdUseCase>().AsSingle();

            Container
                .Bind<IWSCommandsUseCase>()
                .WithId(IWSCommandsUseCase.AuthorizedInstance)
                .To<WSAuthCommandsUseCase>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<AuthDataNavigator>().AsSingle();
            Container.BindInterfacesAndSelfTo<AutoLoginService>().AsSingle();
        }
    }
}