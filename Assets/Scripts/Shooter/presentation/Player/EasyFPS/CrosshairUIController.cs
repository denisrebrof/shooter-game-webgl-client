using UnityEngine;

namespace Shooter.presentation.Player.EasyFPS
{
    public class CrosshairUIController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup root;
        [SerializeField] private RectTransform left;
        [SerializeField] private RectTransform right;
        [SerializeField] private RectTransform top;
        [SerializeField] private RectTransform bottom;

        private Vector2 topPosCrosshair;
        private Vector2 bottomPosCrosshair;
        private Vector2 leftPosCrosshair;
        private Vector2 rightPosCrosshair;

        private float fadeoutValue = 1;

        public Vector2 expandValuesCrosshair;

        private void Awake()
        {
            topPosCrosshair = top.anchoredPosition;
            bottomPosCrosshair = bottom.anchoredPosition;
            leftPosCrosshair = left.anchoredPosition;
            rightPosCrosshair = right.anchoredPosition;
        }


        // Used to expand position of the crosshair or make it disappear when running
        public void CrossHairExpansionWhenWalking(bool shooting, bool moving, bool running, bool aiming)
        {
            if (moving && !shooting)
            {
                expandValuesCrosshair += new Vector2(40, 40) * Time.deltaTime;
                if (!running)
                {
                    //not running
                    expandValuesCrosshair = new Vector2(
                        Mathf.Clamp(expandValuesCrosshair.x, 0, 40),
                        Mathf.Clamp(expandValuesCrosshair.y, 0, 40)
                    );
                    fadeoutValue = Mathf.Lerp(fadeoutValue, aiming ? 0 : 1, Time.deltaTime * 2 * (aiming ? 4 : 1));
                }
                else
                {
                    //running
                    fadeoutValue = Mathf.Lerp(fadeoutValue, 0, Time.deltaTime * 10);
                    expandValuesCrosshair = new Vector2(
                        Mathf.Clamp(expandValuesCrosshair.x, 0, 80),
                        Mathf.Clamp(expandValuesCrosshair.y, 0, 80)
                    );
                }
            }
            else
            {
                //if shooting
                expandValuesCrosshair = Vector2.Lerp(expandValuesCrosshair, Vector2.zero, Time.deltaTime * 5);
                expandValuesCrosshair = new Vector2(
                    Mathf.Clamp(expandValuesCrosshair.x, 0, 40),
                    Mathf.Clamp(expandValuesCrosshair.y, 0, 40)
                );
                fadeoutValue = Mathf.Lerp(fadeoutValue, aiming ? 0 : 1, Time.deltaTime * 2 * (aiming ? 4 : 1));
            }

            root.alpha = fadeoutValue;
            left.anchoredPosition = leftPosCrosshair + Vector2.left * expandValuesCrosshair.x;
            right.anchoredPosition = rightPosCrosshair + Vector2.right * expandValuesCrosshair.x;
            top.anchoredPosition = topPosCrosshair + Vector2.up * expandValuesCrosshair.y;
            bottom.anchoredPosition = bottomPosCrosshair + Vector2.down * expandValuesCrosshair.y;
        }
    }
}