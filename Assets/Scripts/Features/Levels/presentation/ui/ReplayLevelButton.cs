using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.Levels.presentation.ui
{
    [RequireComponent(typeof(Button))]
    public class ReplayLevelButton : MonoBehaviour
    {
        [Inject] private CurrentLevelLoadingNavigator levelLoadingNavigator;

        private void Start() => GetComponent<Button>().onClick.AddListener(OnClick);

        private void OnClick() => levelLoadingNavigator.LoadPrevious();
    }
}