using UnityEngine;

namespace Core.Sound.presentation
{
    public class PlaySoundNavigator : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public void Play(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
