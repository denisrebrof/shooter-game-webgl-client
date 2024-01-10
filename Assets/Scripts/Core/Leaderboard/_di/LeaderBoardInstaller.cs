using Core.Leaderboard.data;
using Core.Leaderboard.presentation;
using UnityEngine;
using Zenject;

namespace Core.Leaderboard._di
{
    public class LeaderBoardInstaller : MonoInstaller
    {
        [SerializeField] private LeaderBoardItemView leaderBoardItemView;

        public override void InstallBindings()
        {
#if PLAYFAB
            Container.BindInterfacesAndSelfTo<PlayfabLeaderBoardRemoteDataSource>().AsSingle();
#else
            Container.BindInterfacesAndSelfTo<StubLeaderBoardRemoteDataSource>().AsSingle();
#endif
            Container.BindInterfacesAndSelfTo<DefaultLeaderBoardRepository>().AsSingle();
            Container
                .BindFactory<LeaderBoardItemView, LeaderBoardItemView.Factory>()
                .FromComponentInNewPrefab(leaderBoardItemView);
        }
    }
}