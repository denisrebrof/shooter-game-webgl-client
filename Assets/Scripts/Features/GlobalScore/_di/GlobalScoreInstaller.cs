using Features.GlobalScore.data;
using Features.GlobalScore.domain;
using Zenject;

namespace Features.GlobalScore._di
{
    public class GlobalScoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GlobalScoreLocalDataSource>().AsSingle();
            Container.Bind<IGlobalScoreRepository>().To<PlayfabGlobalScoreRepository>().AsSingle();
        }
    }
}