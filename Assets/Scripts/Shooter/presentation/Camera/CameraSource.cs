using Shooter.presentation.UI.Game;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Camera
{
    public class CameraSource : MonoBehaviour, ICameraView
    {
        [SerializeField] private UnityEngine.Camera cam;
        [SerializeField] private UnityEngine.Camera cam2;
        [SerializeField] private AudioListener listener;
        [SerializeField] private CameraType type;
        [SerializeField] private CanvasType canvasType = CanvasType.None;
        [Inject] private ICameraRepository cameraRepository;
        [Inject] private ICanvasNavigator canvasNavigator;

        private void Start()
        {
            cameraRepository.SetCamera(this, type);
        }

        public void SetActive(bool active)
        {
            cam.enabled = active;
            if (cam2 != null) cam2.enabled = active;
            listener.enabled = active;
            if (canvasType != CanvasType.None) canvasNavigator.SetCanvasActive(canvasType, active);
        }
    }
}