using Core.Sound.domain;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Core.Sound.presentation
{
    public class InitSound : MonoBehaviour
    {
        [Inject] private ISoundPrefsRepository repository;

        [SerializeField] private AudioMixer mixer;

        private void Start()
        {
            var state = repository.GetSoundEnabledState();
            mixer.SetFloat("MasterVolume", state ? 0 : -80);
        }
    }
}