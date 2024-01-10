using UnityEngine;

namespace Plugins.Platforms.YSDK
{
    public class YandexHit : MonoBehaviour
    {
        #if YANDEX_SDK && !UNITY_EDITOR
        public void Hit(string eventName) => YandexSDK.instance.AddHit(eventName);
        #endif
    }
}