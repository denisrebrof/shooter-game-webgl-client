using Features.Lobby.domain;
using Shooter.data;
using Shooter.domain;
using Shooter.domain.Repositories;
using UnityEngine;
using Zenject;

namespace Shooter._di
{
    public class LobbyInstaller : MonoInstaller
    {
        [SerializeField] private WeaponStoreMonoRepository weaponStoreMonoRepository;
        [SerializeField] private LevelProgressionMonoRepository levelProgressionRepository;
        [SerializeField] private WeaponDataSO weaponData;
        
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<IWeaponStoreRepository>()
                .FromInstance(weaponStoreMonoRepository)
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<ILevelProgressionRepository>()
                .FromInstance(levelProgressionRepository)
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<WeaponDataSO>().FromInstance(weaponData).AsSingle();
            Container.BindInterfacesAndSelfTo<LobbyUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<LobbyGamesUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<WeaponFullDataUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<WeaponStoreOperationsUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<SetWeaponSlotUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<WeaponRewardsUseCase>().AsSingle();
        }
    }
}