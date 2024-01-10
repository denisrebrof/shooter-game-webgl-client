using System.Collections.Generic;
using Plugins.GoogleAnalytics;
using UnityEngine;

namespace Shooter.presentation.Camera
{
    public class CameraManager : MonoBehaviour, ICameraNavigator, ICameraRepository
    {
        [SerializeField] private CameraType activeCam = CameraType.Overview;

        private readonly Dictionary<CameraType, ICameraView> cameras = new();

        void ICameraNavigator.SetActiveCam(CameraType type)
        {
            if (type == activeCam)
                return;

            GoogleAnalyticsSDK.SendStringEvent("set_camera", "type", type.ToString());
            TrySetCamActive(activeCam, false);
            TrySetCamActive(type, true);
            activeCam = type;
        }

        void ICameraRepository.SetCamera(ICameraView cam, CameraType type)
        {
            cameras[type] = cam;

            if (type != activeCam)
            {
                cam.SetActive(false);
                return;
            }

            TrySetCamActive(activeCam, true);
        }

        private void TrySetCamActive(CameraType type, bool active)
        {
            if (!cameras.TryGetValue(type, out var cam)) return;
            cam.SetActive(active);
        }
    }

    public enum CameraType
    {
        Overview,
        Player,
        Killed,
        Finished
    }

    public interface ICameraNavigator
    {
        void SetActiveCam(CameraType type);
    }

    public interface ICameraRepository
    {
        void SetCamera(ICameraView cam, CameraType type);
    }

    public interface ICameraView
    {
        void SetActive(bool active);
    }
}