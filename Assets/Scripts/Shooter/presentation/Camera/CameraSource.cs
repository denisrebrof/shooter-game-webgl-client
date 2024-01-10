using Shooter.presentation.Canvases;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Camera
{
    public class CameraSource : MonoBehaviour, ICameraView
    {
        [Inject] private ICameraRepository cameraRepository;
        [Inject] private ICanvasNavigator canvasNavigator;
        [SerializeField] private UnityEngine.Camera cam;
        [SerializeField] private AudioListener listener;
        [SerializeField] private CameraType type;
        [SerializeField] private CanvasType canvasType = CanvasType.None;

        private void Start() => cameraRepository.SetCamera(this, type);

        public void SetActive(bool active)
        {
            cam.enabled = active;
            listener.enabled = active;
            if (canvasType != CanvasType.None) canvasNavigator.SetCanvasActive(canvasType, active);
        }
    }
}