using System.Collections;
using Core.Sound.data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Sound.presentation
{
    public class PlaySoundNavigator : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AssetReferenceT<SoundsConfig> configRef;

        private SoundsConfig config;
        private Coroutine loader;

        private void Start()
        {
            loader = StartCoroutine(LoadConfig());
        }

        public void Play(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }

        public void Play(SoundType type)
        {
            if (config == null)
                return;

            var clip = config.Get(type);
            audioSource.Stop();
            audioSource.PlayOneShot(clip);
            Debug.Log("PlaySound: "  + type + " " + clip.name);
        }

        private IEnumerator LoadConfig()
        {
            var handle = Addressables.LoadAssetAsync<SoundsConfig>(configRef);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
                config = handle.Result;

            // Освобождение ресурсов
            Addressables.Release(handle);
        }

        private void OnDestroy()
        {
            if (loader != null) StopCoroutine(loader);
        }
    }
}