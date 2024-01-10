using UnityEngine;
using UnityEngine.Audio;

namespace Core.Ads.data
{
    [CreateAssetMenu(menuName = "Settings/AdsSettings")]
    public class AdsSettings: ScriptableObject
    {
        public AudioMixer muteAudioMixer;
    }
}