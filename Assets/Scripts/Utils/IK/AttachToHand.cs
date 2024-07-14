using System;
using System.Collections;
using UnityEngine;

public class AttachToHand : MonoBehaviour
{
    [SerializeField] private bool breakOnAttach;
    [SerializeField] private Transform source;
    [SerializeField] private string handName = "mag_target_l";
    [SerializeField] private float attachDelay = 0.5f;
    [SerializeField] private float attachDuration = 0.5f;
    [SerializeField] private float beforeDetachDelay = 1f;
    [SerializeField] private float detachDuration = 0.5f;

    [SerializeField] private Vector3 offsetPosition;
    [SerializeField] private Vector3 offsetRotation;

    private Quaternion offsetRotationQ;

    private Vector3 InHandPos => target.localToWorldMatrix.MultiplyPoint(offsetPosition);
    private Quaternion InHandRot => target.rotation * offsetRotationQ;

    private Transform target;
    private Vector3 defaultPos;
    private Quaternion defaultRot;

    private Coroutine attachCoroutine;

#if UNITY_EDITOR
    [ContextMenu("Capture Offsets")]
    public void Capture()
    {
        offsetPosition = target.worldToLocalMatrix.MultiplyPoint(source.position);
        offsetRotation = (Quaternion.Inverse(target.rotation) * source.rotation).eulerAngles;
    }
#endif

    private void Start()
    {
        offsetRotationQ = Quaternion.Euler(offsetRotation);
        defaultPos = source.localPosition;
        defaultRot = source.localRotation;
        var handObject = GameObject.Find(handName);
        if (handObject == null)
            return;

        target = handObject.transform;
    }

    private IEnumerator AttachCoroutine()
    {
        source.localPosition = defaultPos;
        source.localRotation = defaultRot;

        yield return new WaitForSeconds(attachDelay);

        var timer = attachDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            var progress = 1f - timer / attachDuration;
            source.position = Vector3.Lerp(source.position, InHandPos, progress);
            source.rotation = Quaternion.Slerp(source.rotation, InHandRot, progress);
            yield return null;
        }

        timer = beforeDetachDelay;
        while (timer > 0)
        {
            source.position = InHandPos;
            source.rotation = InHandRot;
            timer -= Time.deltaTime;
            yield return null;
        }

        timer = detachDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            var progress = 1f - timer / detachDuration;
            source.localPosition = Vector3.Lerp(source.localPosition, defaultPos, progress);
            source.localRotation = Quaternion.Slerp(source.localRotation, defaultRot, progress);
            yield return null;
        }

        source.localPosition = defaultPos;
        source.localRotation = defaultRot;
    }

    public void Attach()
    {
        if (attachCoroutine != null) StopCoroutine(attachCoroutine);
        attachCoroutine = StartCoroutine(AttachCoroutine());
#if UNITY_EDITOR
        if (breakOnAttach) Debug.Break();
#endif
    }

    private void OnDestroy()
    {
        if (attachCoroutine != null) StopCoroutine(attachCoroutine);
    }
}