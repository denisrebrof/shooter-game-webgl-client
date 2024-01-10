using System;
using Shooter.domain;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI
{
    public class SimpleGameStateTypeText : MonoBehaviour
    {
        [Inject] private IGameStateRepository gameStateRepository;

        [SerializeField] private TMP_Text text;
        
        private IDisposable handler = Disposable.Empty;

        private void OnEnable()
        {
            handler = gameStateRepository
                .state
                .Select(state => state.Type)
                .Subscribe(HandleGameStateType)
                .AddTo(this);
        }

        private void OnDisable() => handler.Dispose();

        private void HandleGameStateType(GameStateTypes stateType)
        {
            text.text = stateType.ToString();
        }
    }
}