using Shooter.data;
using Shooter.domain;
using Shooter.domain.Repositories;
using Shooter.presentation.Camera;
using Shooter.presentation.Canvases;
using Shooter.presentation.Player;
using Shooter.presentation.Player.EasyFPS;
using Shooter.presentation.Player.Weapons;
using Shooter.presentation.UI;
using Shooter.presentation.UI.MiniMap;
using Shooter.presentation.UI.Pause;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Shooter._di
{
    public class ShooterInstaller : MonoInstaller
    {
        [SerializeField] private GameStateMonoRepository gameStateMonoRepository;
        [SerializeField] private GameActionsMonoRepository gameActionsMonoRepository;
        [FormerlySerializedAs("weaponPrefabs")] [SerializeField] private WeaponDataSO weaponData;
        [SerializeField] private CameraManager camManager;
        [SerializeField] private PauseMenuController pauseMenuController;
        [SerializeField] private KilledCameraController killedCameraController;
        [SerializeField] private CanvasManager canvasManager;
        [SerializeField] private SimpleBulletPool bulletPool;
        [SerializeField] private SimplePlayerBulletPool bulletPoolPlayer;
        [SerializeField] private Canvas playerCanvas;
        [SerializeField] private WeaponsPanelController weaponsPanelController;
        [SerializeField] private MinimapController minimapController;
        [SerializeField] private CrosshairUIController crosshairUIController;

        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<IGameStateRepository>()
                .FromInstance(gameStateMonoRepository)
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<IGameActionsRepository>()
                .FromInstance(gameActionsMonoRepository)
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<WeaponDataSO>().FromInstance(weaponData).AsSingle();
            
            Container.BindInterfacesAndSelfTo<GamePlayerUpdatesUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<GamePlayerUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<CurrentPlayerStateUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<GamePlayerAliveStateUpdatesUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<CurrentPlayerAliveStateUpdatesUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShootPlayerUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<SelectWeaponUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<CurrentPlayerHpUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<LeaveMatchUseCase>().AsSingle();

            Container.BindInterfacesAndSelfTo<CameraManager>().FromInstance(camManager).AsSingle();
            Container.BindInterfacesAndSelfTo<IPausedStateHandler>().FromInstance(pauseMenuController).AsSingle();
            Container.Bind<KilledCameraController>().FromInstance(killedCameraController).AsSingle();
            Container.Bind<ICanvasNavigator>().FromInstance(canvasManager).AsSingle();
            Container.Bind<SimplePlayerBulletPool>().FromInstance(bulletPoolPlayer).AsSingle();
            Container.Bind<SimpleBulletPool>().FromInstance(bulletPool).AsSingle();
            Container.Bind<Canvas>().WithId("PlayerCanvas").FromInstance(playerCanvas).AsSingle();
            Container.BindInterfacesAndSelfTo<WeaponsPanelController>().FromInstance(weaponsPanelController).AsSingle();
            Container.BindInterfacesAndSelfTo<MinimapController>().FromInstance(minimapController).AsSingle();
            Container.BindInterfacesAndSelfTo<CrosshairUIController>().FromInstance(crosshairUIController).AsSingle();
        }
    }
}