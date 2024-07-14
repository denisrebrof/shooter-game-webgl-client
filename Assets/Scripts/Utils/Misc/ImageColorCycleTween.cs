using UnityEngine;
using UnityEngine.UI;

namespace Utils.Misc
{
    public class ImageColorCycleTween : MonoBehaviour
    {
        [SerializeField] private Image target;
        [SerializeField] private Color start;
        [SerializeField] private Color end;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float duration = 1f;

        private float time;
        private bool inOrOut;

        private float inverseDuration;

        private void Awake() => inverseDuration = 1f / duration;

        private void OnEnable() => time = 0f;

        private void Update()
        {
            Tick();
            Evaluate();
        }

        private void Evaluate()
        {
            var progressTime = inOrOut ? time : 1f - time;
            var progress = curve.Evaluate(progressTime * inverseDuration);
            target.color = Color.Lerp(start, end, progress);
        }

        private void Tick()
        {
            time += Time.deltaTime;
            if (!(time > duration)) return;
            inOrOut = !inOrOut;
            time = time % duration;
        }
    }
}