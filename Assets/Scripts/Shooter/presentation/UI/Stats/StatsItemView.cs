using Core.User.domain;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Shooter.presentation.UI.Stats
{
    public class StatsItemView : MonoBehaviour
    {
        [Inject] private UserDataRepository userDataRepository;

        [SerializeField] private TMP_Text nick;
        [SerializeField] private TMP_Text kills;
        [SerializeField] private TMP_Text death;
        [SerializeField] private Image currentPlayerHighlight;

        private long cachedPlayerId = -1;

        public void SetData(long playerId, int killsCount, int deathCount, bool isCurrentPlayer)
        {
            kills.text = killsCount.ToString();
            death.text = deathCount.ToString();
            if (cachedPlayerId == playerId)
                return;

            nick.text = "";
            LoadUserNick(playerId);
            currentPlayerHighlight.enabled = isCurrentPlayer;
            cachedPlayerId = playerId;
        }

        private void LoadUserNick(long playerId) => userDataRepository
            .LoadUser(playerId)
            .Subscribe(data => nick.text = data.username)
            .AddTo(this);
    }
}