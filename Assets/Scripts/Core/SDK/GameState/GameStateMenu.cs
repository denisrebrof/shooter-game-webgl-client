using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject;

namespace Core.SDK.GameState
{
    public class GameStateMenu : MonoBehaviour
    {
        [Inject] private GameStateNavigator gameStateNavigator;
        private string menuName;
        private static Dictionary<string, bool> menuNamesToStates = new();

        private void OnEnable()
        {
            menuName ??= gameObject.name;
            menuNamesToStates[menuName] = true;
            gameStateNavigator.SetMenuShownState(menuNamesToStates.Any(_ => _.Value));
        }

        private void OnDisable()
        {
            menuName ??= gameObject.name;
            menuNamesToStates[menuName] = false;
            gameStateNavigator.SetMenuShownState(menuNamesToStates.Any(_ => _.Value));
        }
    }
}