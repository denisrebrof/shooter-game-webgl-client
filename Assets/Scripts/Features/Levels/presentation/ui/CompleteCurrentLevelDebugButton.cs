using Features.Levels.domain;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.Levels.presentation.ui
{
    [RequireComponent(typeof(Button))]
    public class CompleteCurrentLevelDebugButton : MonoBehaviour
    {
        [Inject] private CompleteCurrentLevelUseCase completeCurrentLevelUseCase;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(
                () => completeCurrentLevelUseCase.CompleteCurrentLevel()
            );
        }
    }
}