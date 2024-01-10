using UnityEngine;
using UnityEngine.Events;

namespace Utils.Misc
{
    public class StartListener : MonoBehaviour
    {
        [SerializeField] private UnityEvent onStart;

        private void Start() => onStart?.Invoke();
    }
}
