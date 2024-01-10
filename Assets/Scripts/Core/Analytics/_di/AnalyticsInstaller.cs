using Core.Analytics.adapter;
using UnityEngine;
using Zenject;
#if GAME_ANALYTICS
#endif

namespace Core.Analytics._di
{
    [CreateAssetMenu(menuName = "Installers/AnalyticsInstaller")]
    public class AnalyticsInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings() => BindAnalyticsAdapter();

        private void BindAnalyticsAdapter()
        {
            Container
                .Bind<AnalyticsAdapter>()
#if GAME_ANALYTICS
                .To<GameAnalyticsAdapter>()
#elif PLAYFAB_ANALYTICS
                .To<PlayfabAnalyticsAdapter>()
#elif DEBUG_ANALYTICS
                .FromInstance(new DebugLogAnalyticsAdapter(true))
#else
                .FromInstance(new DebugLogAnalyticsAdapter(false))
#endif
                .AsSingle();
        }
    }
}