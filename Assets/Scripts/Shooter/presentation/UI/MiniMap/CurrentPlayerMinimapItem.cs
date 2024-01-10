using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI.MiniMap
{
    public class CurrentPlayerMinimapItem : MonoBehaviour
    {
        [Inject] private IMinimapItemHandler handler;

        private void Awake() => handler.SetPlayer(transform);
    }
}