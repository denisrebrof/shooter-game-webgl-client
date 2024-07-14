using Michsky.MUIP;
using UnityEngine;
using Zenject;

namespace Core.Sound.presentation
{
    [RequireComponent(typeof(ButtonManager))]
    public class MUIPButtonSound : MonoBehaviour 
    {
        [Inject] private PlaySoundNavigator playSoundNavigator;

        [SerializeField, HideInInspector] private ButtonManager target;
        [SerializeField] private SoundType type = SoundType.ButtonDefault;

        private void Reset()
        {
            target = GetComponent<ButtonManager>();
        }

        private void Start()
        {
            target.onClick.AddListener(() => playSoundNavigator.Play(type));
        }
    }
}