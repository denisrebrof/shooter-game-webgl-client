using System.Collections;
#if YANDEX_SDK
using Plugins.Platforms.YSDK;
#endif
using UnityEngine;
using Zenject;

namespace Core.SDK
{
    public class GameReadyApi : MonoBehaviour
    {
#if YANDEX_SDK
        [Inject] private YandexSDK sdk;
#endif

        private bool gameReadyApiInvoked;

        private IEnumerator InvokeGRA()
        {
            yield return new WaitForSeconds(0.5f);
#if YANDEX_SDK && !UNITY_EDITOR
            sdk.InvokeGameReadyApi();
#endif
        }

        private void Start()
        {
            if (gameReadyApiInvoked)
                return;

#if YANDEX_SDK
            StartCoroutine(InvokeGRA());
#endif
            gameReadyApiInvoked = true;
        }
    }
}