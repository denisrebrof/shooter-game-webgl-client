using Core.Leaderboard.domain;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Leaderboard.presentation
{
    public class LeaderBoardItemView: MonoBehaviour
    {
        [SerializeField] private Text posText;
        [SerializeField] private Text scoreText;
        [SerializeField] private Color currentUserTextColor;
        [SerializeField] private Color defTextColor;
        [SerializeField] private Outline currentUserOutline;
        [SerializeField] private Outline scoreOutline;

        public void Setup(LeaderBoardItem item, bool isCurrentPlayer)
        {
            posText.text = item.Position + ". " + item.PlayerName;
            scoreText.text = item.Score.ToString();
            posText.color = isCurrentPlayer ? currentUserTextColor : defTextColor;
            scoreText.color = isCurrentPlayer ? currentUserTextColor : defTextColor;
            currentUserOutline.enabled = isCurrentPlayer;
            scoreOutline.enabled = isCurrentPlayer;
        }
        
        public class Factory : PlaceholderFactory<LeaderBoardItemView>
        {
        }
    }
}