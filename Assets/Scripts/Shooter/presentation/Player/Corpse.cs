using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shooter.presentation.Player
{
    public class Corpse : MonoBehaviour
    {
        [SerializeField] private Animator corpseAnimator;
        [SerializeField] private Transform sourceRoot;
        [SerializeField] private Transform corpseRoot;
        [SerializeField] private List<Transform> sourceBones;
        [SerializeField] private List<Transform> corpseBones;
        [SerializeField] private List<TransformData> animationStartPositions;
        [SerializeField] private float lerpDelay = 0.5f;

        private TransformData[] initialPositions;

        [ContextMenu("Grab Bones")]
        private void GrabBones()
        {
            sourceBones = sourceRoot.GetComponentsInChildren<Transform>().ToList();
            corpseBones = corpseRoot.GetComponentsInChildren<Transform>().ToList();
        }

        [ContextMenu("Capture Animation Start")]
        private void CaptureAnimationStart()
        {
            animationStartPositions = corpseBones.Select(GetData).ToList();
        }

        private static TransformData GetData(Transform source) => new(source.localPosition, source.localRotation);

        private void OnEnable()
        {
            initialPositions ??= new TransformData[corpseBones.Count];
            corpseAnimator.enabled = false;
            SyncArmature();
            StartCoroutine(LerpBonesToAnimation());
        }

        private IEnumerator LerpBonesToAnimation()
        {
            var timer = lerpDelay;
            while (timer > 0f)
            {
                for (var i = 0; i < corpseBones.Count; i++)
                {
                    var corpseBone = corpseBones[i];
                    var time = 1f - timer / lerpDelay;
                    var initialPos = initialPositions[i];
                    var targetPos = animationStartPositions[i];
                    corpseBone.localPosition = Vector3.Lerp(initialPos.Pos, targetPos.Pos, time);
                    corpseBone.localRotation = Quaternion.Slerp(initialPos.Rot, targetPos.Rot, time);
                }

                timer -= Time.deltaTime;
                yield return null;
            }

            corpseAnimator.enabled = true;
        }

        private void SyncArmature()
        {
            // corpseRoot.position = sourceRoot.position;
            // corpseRoot.rotation = sourceRoot.rotation;
            for (var i = 0; i < sourceBones.Count; i++)
            {
                var sourceBone = sourceBones[i];
                var corpseBone = corpseBones[i];
                var data = new TransformData(sourceBone.localPosition, sourceBone.localRotation);
                corpseBone.localPosition = data.Pos;
                corpseBone.localRotation = data.Rot;
                initialPositions[i] = data;
            }
        }

        [Serializable]
        public struct TransformData
        {
            public Vector3 Pos;
            public Quaternion Rot;

            public TransformData(Vector3 pos, Quaternion rot)
            {
                Pos = pos;
                Rot = rot;
            }
        }
    }
}