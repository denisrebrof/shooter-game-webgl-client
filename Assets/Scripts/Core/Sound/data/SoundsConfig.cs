using Core.Sound.presentation;
using UnityEngine;

namespace Core.Sound.data
{
    [CreateAssetMenu(menuName = "SoundConfig")]
    public class SoundsConfig : ScriptableObject
    {
        [SerializeField] private AudioClip defaultClip;
        [SerializeField] private SerializableDictionary<SoundType, AudioClip> clips = new();

        public AudioClip Get(SoundType type) => clips.TryGetValue(type, out var clip) ? clip : defaultClip;
    }
}