using System.Linq;
using Core.SDK.GameState;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI.Game
{
    public class CanvasManager : MonoBehaviour, ICanvasNavigator
    {
        [SerializeField] private SerializableDictionary<CanvasType, Canvas> canvases;
        [Inject] private GameStateNavigator gameStateNavigator;

        private void Awake()
        {
            canvases.Values.ToList().ForEach(canvas => canvas.enabled = false);
        }

        public void SetCanvasActive(CanvasType type, bool active)
        {
            if (!canvases.TryGetValue(type, out var canvas))
                return;

            canvas.enabled = active;
            if (active && type == CanvasType.Finished) gameStateNavigator.SetLevelPlayingState(false);
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