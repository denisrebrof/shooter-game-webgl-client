using UnityEngine;
using UnityEngine.Events;

namespace Features.Hints.presentation
{
    public class HintsNavigator : MonoBehaviour
    {
        [SerializeField] private UnityEvent showHint;
        [SerializeField] private UnityEvent hideHint;

        public void Show() => showHint?.Invoke();

        public void Hide() => hideHint?.Invoke();
    }
}