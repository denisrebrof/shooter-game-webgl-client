using System.Collections.Generic;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Scene
{
    public class PlayingStateActivator : MonoBehaviour
    {
        [SerializeField] private List<GameObject> playingStateObjects;
        [Inject] private IGameStateRepository gameStateRepository;

        private void Awake()
        {
            gameStateRepository
                .state
                .Select(state => state.Type == GameStateTypes.Playing)
                .DistinctUntilChanged()
                .Subscribe(SetObjectsActive)
                .AddTo(this);
        }

        private void SetObjectsActive(bool active)
        {
            playingStateObjects.ForEach(playingObject => playingObject.SetActive(active));
        }
    }
}