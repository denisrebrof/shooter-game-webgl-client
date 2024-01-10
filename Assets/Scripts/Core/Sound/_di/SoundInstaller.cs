using Core.Sound.data;
using Core.Sound.domain;
using Core.Sound.presentation;
using UnityEngine;
using Zenject;

namespace Core.Sound._di
{
    [CreateAssetMenu(menuName = "Installers/SoundInstaller")]
    public class SoundInstaller: ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISoundPrefsRepository>().To<PlayerPrefsSoundPrefsRepository>().FromNew().AsSingle();
            var playSoundNavigator = FindObjectOfType<PlaySoundNavigator>();
            Container.BindInstance(playSoundNavigator).AsSingle();
        }
    }
}