using UnityEngine;
using Utils.WebSocketClient.data;
using Utils.WebSocketClient.domain;
using Zenject;

namespace Utils.WebSocketClient._di
{
    [CreateAssetMenu(menuName = "Installers/WebSocketInstaller")]
    public class WebSocketInstaller : ScriptableObjectInstaller
    {
        public WSSettings prodSettings;
        public WSSettings stageSettings;
        public bool useStage;

        public WSSettings Settings
        {
            get
            {
#if UNITY_EDITOR
                return useStage ? stageSettings : prodSettings;
#else
                return prodSettings;
#endif
            }
        }

        public override void InstallBindings()
        {
            Container.Bind<WSSettings>().FromInstance(Settings).AsSingle();

            var socketStorage = new WebSocketStorage();
            Container.Bind<WebSocketStorage>().FromInstance(socketStorage).AsSingle();
            Container.BindInterfacesAndSelfTo<InMemoryWSCommandsRepository>().AsSingle();

            var wsConnectionRepository = FindObjectOfType<MonoWSConnectionRepository>();
            Container.Bind<IWSConnectionRepository>().FromInstance(wsConnectionRepository).AsSingle();

            Container
                .Bind<IWSCommandsUseCase>()
                .WithId(IWSCommandsUseCase.BaseInstance)
                .To<WSCommandsUseCase>()
                .AsSingle();
        }
    }
}