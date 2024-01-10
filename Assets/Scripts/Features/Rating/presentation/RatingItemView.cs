using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Pooling;

namespace Shooter.presentation.UI.Rating
{
    public class RatingItemView : MonoPoolItem
    {
        [SerializeField] private TMP_Text usernameText;
        [SerializeField] private TMP_Text ratingText;
        [SerializeField] private TMP_Text posText;
        [SerializeField] private Image mineMark;

        public void Setup(string username, int rating, int pos, bool isMine)
        {
            usernameText.text = username;
            ratingText.text = rating.ToString();
            posText.text = pos.ToString();
            mineMark.enabled = isMine;
        }
    }
}