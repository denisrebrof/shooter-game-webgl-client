using System;
using Core.Sound.domain;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Core.Ads.presentation.InterstitialAdNavigator.decorators
{
    public class InterstitialAdNavigatorMuteAudioDecorator : MonoBehaviour, IInterstitialAdNavigator
    {
        [Inject] private IInterstitialAdNavigator target;
        [Inject] private ISoundPrefsRepository soundPrefsRepository;
        [SerializeField] private AudioMixer mixer;

        public IObservable<ShowInterstitialResult> ShowAd()
        {
            mixer.SetFloat("MasterVolume", -80);
            return target.ShowAd().DoOnCompleted(
                () =>
                {
                    var state = soundPrefsRepository.GetSoundEnabledState();
                    mixer.SetFloat("MasterVolume", state ? 0 : -80);
                }
            );
        }
    }
}