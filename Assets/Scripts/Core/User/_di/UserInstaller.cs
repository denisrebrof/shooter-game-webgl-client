using Core.User.data;
using Core.User.domain;
using Zenject;

namespace Core.User._di
{
    public class UserInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UserNameLocalDataSource>().ToSelf().AsSingle();
            Container.Bind<UserDataRepository>().ToSelf().AsSingle();
            Container
                .Bind<ICurrentUserNameRepository>()
#if PLAYFAB
                .To<PlayfabUserNameRepository>()
#else
                // .To<LocalUserNameRepository>()
                .To<WebSocketCurrentUserNameRepository>()
#endif
                .AsSingle();
            Container.Bind<ValidateUserNameUseCase>().ToSelf().AsSingle();
        }
    }
}