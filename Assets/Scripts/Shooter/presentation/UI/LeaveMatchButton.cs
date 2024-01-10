using Shooter.domain;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Shooter.presentation.UI
{
    public class LeaveMatchButton : MonoBehaviour
    {
        [Inject] private LeaveMatchUseCase leaveMatchUseCase;
        [SerializeField] private Button target;
        private void Start() => target.onClick.AddListener(Leave);

        private void Leave() => leaveMatchUseCase.Leave().AddTo(this);
    }
}