using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Pooling;

namespace Features.Rating.presentation
{
    public class RatingItemView : MonoPoolItem
    {
        [SerializeField] private TMP_Text usernameText;
        [SerializeField] private TMP_Text ratingText;
        [SerializeField] private TMP_Text posText;
        [SerializeField] private CanvasGroup group;
        [SerializeField] private Image mineMark;

        public void Setup(string username, int rating, int pos, bool isMine)
        {
            usernameText.text = username;
            ratingText.text = rating.ToString();
            posText.text = pos.ToString();
            mineMark.enabled = isMine;
            group.alpha = isMine ? 1f : 0.6f;
        }
    }
}