using System;
using UniRx;
using UnityEngine;
using Zenject;
#if CRAZY_SDK
using CrazyGames;
#endif

namespace Core.SDK.GameState
{
    public class GameStateHandler : MonoBehaviour
    {
        [Inject] private GameStateNavigator gameStateNavigator;

#if CRAZY_SDK
        private void Start() => gameStateNavigator.GetGameState().Subscribe(HandleGameState).AddTo(this);

        private static void HandleGameState(GameState state)
        {
            switch (state)
            {
                case GameState.Active:
                    CrazySDK.Instance.GameplayStart();
                    break;
                case GameState.Disabled:
                    CrazySDK.Instance.GameplayStop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
#endif
    }
}