using System.Linq;
using Core.SDK.GameState;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Canvases
{
    public class CanvasManager : MonoBehaviour, ICanvasNavigator
    {
        [Inject] private GameStateNavigator gameStateNavigator;

        [SerializeField] private SerializableDictionary<CanvasType, Canvas> canvases;

        private void Awake()
        {
            canvases.Values.ToList().ForEach(canvas => canvas.enabled = false);
        }

        public void SetCanvasActive(CanvasType type, bool active)
        {
            if (!canvases.TryGetValue(type, out var canvas))
                return;

            canvas.enabled = active;
            if (active && type == CanvasType.Finished)
            {
                gameStateNavigator.SetLevelPlayingState(false);
            }
        }
    }

    public enum CanvasType
    {
        None,
        Player,
        Killed,
        Preparing,
        Finished
    }

    public interface ICanvasNavigator
    {
        void SetCanvasActive(CanvasType type, bool active);
    }
}