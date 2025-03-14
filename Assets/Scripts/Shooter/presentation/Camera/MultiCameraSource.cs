﻿using System.Collections.Generic;
using Shooter.presentation.UI.Game;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Camera
{
    public class MultiCameraSource : MonoBehaviour, ICameraView
    {
        [SerializeField] private List<UnityEngine.Camera> cameras = new();
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
            cameras.ForEach(cam => cam.enabled = active);
            listener.enabled = active;
            if (canvasType != CanvasType.None) canvasNavigator.SetCanvasActive(canvasType, active);
        }
    }
}