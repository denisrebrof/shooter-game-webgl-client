using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Shooter.presentation.Player.Weapons
{
    public class HandPoseController : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<string, HandPoseData> poses;
        [SerializeField] private float switchPoseDurationS = 3f;

        [SerializeField] private Transform thumb;
        [SerializeField] private Transform pointer;
        [SerializeField] private Transform middle;
        [SerializeField] private Transform ring;
        [SerializeField] private Transform little;

        public string targetPose;

        private string prevTargetPose;
        [CanBeNull] private Coroutine switchPoseCoroutine;

        private void Update()
        {
            if (prevTargetPose == targetPose || !poses.ContainsKey(targetPose))
                return;

            prevTargetPose = targetPose;
            switchPoseCoroutine = StartCoroutine(SwitchPoseCoroutine());
        }

        private void OnDisable()
        {
            if (switchPoseCoroutine == null)
                return;

            StopCoroutine(switchPoseCoroutine);
        }

        private HandPoseData GetCurrentPose()
        {
            return new HandPoseData
            {
                thumb = HandPoseItemData.Capture(thumb),
                pointer = HandPoseItemData.Capture(pointer),
                middle = HandPoseItemData.Capture(middle),
                ring = HandPoseItemData.Capture(ring),
                little = HandPoseItemData.Capture(little)
            };
        }

        private IEnumerator SwitchPoseCoroutine()
        {
            var startPose = GetCurrentPose();
            var finishPose = poses[targetPose];
            var timer = 0f;
            while (timer < switchPoseDurationS)
            {
                var progress = timer / switchPoseDurationS;
                var pose = HandPoseData.Lerp(startPose, finishPose, progress);
                ApplyPose(pose);
                timer += Time.deltaTime;
                yield return null;
            }

            ApplyPose(finishPose);
        }

        private void ApplyPose(HandPoseData pose)
        {
            pose.thumb.Apply(thumb);
            pose.pointer.Apply(pointer);
            pose.middle.Apply(middle);
            pose.ring.Apply(ring);
            pose.little.Apply(little);
        }

        [Serializable]
        private struct HandPoseData
        {
            [SerializeField] public HandPoseItemData thumb;
            [SerializeField] public HandPoseItemData pointer;
            [SerializeField] public HandPoseItemData middle;
            [SerializeField] public HandPoseItemData ring;
            [SerializeField] public HandPoseItemData little;

            public static HandPoseData Lerp(HandPoseData a, HandPoseData b, float t)
            {
                return new HandPoseData
                {
                    thumb = HandPoseItemData.Lerp(a.thumb, b.thumb, t),
                    pointer = HandPoseItemData.Lerp(a.pointer, b.pointer, t),
                    middle = HandPoseItemData.Lerp(a.middle, b.middle, t),
                    ring = HandPoseItemData.Lerp(a.ring, b.ring, t),
                    little = HandPoseItemData.Lerp(a.little, b.little, t)
                };
            }
        }

        [Serializable]
        private struct HandPoseItemData
        {
            [SerializeField] public Vector3 position;
            [SerializeField] public Vector3 rotation;
            [SerializeField] public Vector3 scale;

            public void Apply(Transform target)
            {
                target.localPosition = position;
                target.localRotation = Quaternion.Euler(rotation);
                target.localScale = scale;
            }

            public static HandPoseItemData Capture(Transform source)
            {
                return new HandPoseItemData
                {
                    position = source.localPosition,
                    rotation = source.localRotation.eulerAngles,
                    scale = source.localScale
                };
            }

            public static HandPoseItemData Lerp(HandPoseItemData a, HandPoseItemData b, float t)
            {
                return new HandPoseItemData
                {
                    position = Vector3.Lerp(a.position, b.position, t),
                    rotation = Vector3.Lerp(a.rotation, b.rotation, t),
                    scale = Vector3.Lerp(a.scale, b.scale, t)
                };
            }
        }
#if UNITY_EDITOR
        [SerializeField] private string capturedPoseName;
        [SerializeField] private string applyPoseName;
#endif

#if UNITY_EDITOR
        [ContextMenu("Generate")]
        private void Capture()
        {
            var data = GetCurrentPose();
            poses[capturedPoseName] = data;
        }

        [ContextMenu("ApplySelected")]
        private void ApplySelected()
        {
            if (!poses.TryGetValue(applyPoseName, out var data))
            {
                Debug.LogError("Cannot find apply pose");
                return;
            }

            ApplyPose(data);
        }
#endif
    }
}