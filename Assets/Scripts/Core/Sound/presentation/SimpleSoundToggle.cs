using Core.Sound.domain;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zenject;

namespace Core.Sound.presentation
{
    [RequireComponent(typeof(Toggle))]
    public class SimpleSoundToggle : MonoBehaviour
    {
        [Inject] private PlaySoundNavigator playSoundNavigator;
        [Inject] private ISoundPrefsRepository repository;
        [SerializeField] private AudioMixer mixer;

        private Toggle toggle;

        private void Awake() => toggle = GetComponent<Toggle>();

        private void Start()
        {
            var state = repository.GetSoundEnabledState();
            toggle.isOn = state;
            UpdateAudio(state);
            toggle.onValueChanged.AddListener(UpdateAudio);
        }

        private void UpdateAudio(bool state)
        {
            Debug.Log("UpdateAudio: " + state);
            mixer.SetFloat("MasterVolume", state ? 0 : -80);
            repository.SetSoundEnabledState(state);
            playSoundNavigator.Play(SoundType.ButtonOk);
        }
    }
}
