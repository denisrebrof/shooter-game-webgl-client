#if POKI_SDK
using Core.SDK.HappyTime;
using Poki.Plugins.Platforms.Poki;
using Zenject;

namespace SDK.HappyTime.controller
{
    public class PokiHappyTimeController: IHappyTimeController
    {
        [Inject] private PokiUnitySDK sdk;

        public void SetHappyTime(float intencity = 0.5f)
        {
            sdk.happyTime(intencity);
        }
    }
}
#endif
