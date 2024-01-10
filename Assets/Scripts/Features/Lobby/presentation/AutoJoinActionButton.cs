using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.Lobby.presentation
{
    public class AutoJoinActionButton : MonoBehaviour
    {
        [SerializeField] private Button target;
        [SerializeField] private AutoJoinAction action;

        private void Awake() => target
            .onClick
            .AddListener(() => AutoJoinUseCase.Instance.SetAction(action));
    }
}