using Shooter.domain;
using Shooter.domain.Model;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace Shooter.presentation.UI.Game
{
    public class PlayerTeamText : MonoBehaviour
    {
        [Inject] private CurrentPlayerTeamUseCase playerTeamUseCase;
        [SerializeField] private LocalizedString redTeam;
        [SerializeField] private LocalizedString blueTeam;
        [SerializeField] private TMP_Text target;

        private void Awake() => playerTeamUseCase
            .TeamFlow
            .Subscribe(SetTeam)
            .AddTo(this);

        private void SetTeam(Teams team)
        {
            var localizedString = team == Teams.Red ? redTeam : blueTeam;
            target.text = localizedString.GetLocalizedString();
        }
    }
}