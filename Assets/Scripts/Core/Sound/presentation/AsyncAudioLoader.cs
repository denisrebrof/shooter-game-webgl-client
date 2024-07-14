using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Sound.presentation
{
    public class AsyncAudioLoader : MonoBehaviour
    {
        // Ссылка на Addressable Asset для аудиоклипа
        public AssetReferenceT<AudioClip> audioClipReference;

        // Аудио источник, который будет воспроизводить загруженный клип
        [SerializeField] private AudioSource audioSource;

        private void Start()
        {
            // Запускаем асинхронную загрузку аудио
            StartCoroutine(LoadAndPlayAudio());
        }

        private IEnumerator LoadAndPlayAudio()
        {
            // Асинхронно загружаем аудиоклип
            AsyncOperationHandle<AudioClip> handle = audioClipReference.LoadAssetAsync<AudioClip>();

            // Ждем завершения загрузки
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                // Получаем загруженный аудиоклип
                AudioClip clip = handle.Result;

                // Назначаем клип аудио источнику и начинаем воспроизведение
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

        private void OnDestroy()
        {
            // Освобождаем ресурсы, когда объект уничтожается
            if (audioClipReference != null)
            {
                audioClipReference.ReleaseAsset();
            }
        }
    }
}