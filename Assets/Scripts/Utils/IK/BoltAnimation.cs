using System.Collections;
using UnityEngine;

public class BoltAnimation : MonoBehaviour
{
    [SerializeField] private Transform source;
    [SerializeField] private Vector3 offset;
    [Header("Reload")] [SerializeField] private AnimationCurve tweenInReload = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private AnimationCurve tweenOutReload = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float delayReload = 1f;
    [SerializeField] private float durationInReload = 0.5f;
    [SerializeField] private float durationOutReload = 0.5f;
    [Header("Fire")] [SerializeField] private AnimationCurve tweenInFire = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private AnimationCurve tweenOutFire = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float delayFire = 0f;
    [SerializeField] private float durationInFire = 0.1f;
    [SerializeField] private float durationOutFire = 0.1f;

    private Vector3 defaultPos;

    private Coroutine animCoroutine;

    private void Start()
    {
        defaultPos = source.localPosition;
    }

    private IEnumerator AnimCoroutine(bool fireOrReload)
    {
        var offsetPos = defaultPos + offset;
        source.localPosition = defaultPos;
        var delay = fireOrReload ? delayFire : delayReload;
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        var durationIn = fireOrReload ? durationInFire : durationInReload;
        var tweenIn = fireOrReload ? tweenInFire : tweenInReload;
        var timer = durationIn;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            var progress = tweenIn.Evaluate(1f - timer / durationIn);
            source.localPosition = Vector3.Lerp(defaultPos, offsetPos, progress);
            yield return null;
        }

        var durationOut = fireOrReload ? durationOutFire : durationOutReload;
        var tweenOut = fireOrReload ? tweenOutFire : tweenOutReload;
        timer = durationOut;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            var progress = tweenOut.Evaluate(1f - timer / durationOut);
            source.localPosition = Vector3.Lerp(offsetPos, defaultPos, progress);
            yield return null;
        }

        source.localPosition = defaultPos;
    }

    public void Animate(bool fireOrReload)
    {
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(AnimCoroutine(fireOrReload));
    }
}