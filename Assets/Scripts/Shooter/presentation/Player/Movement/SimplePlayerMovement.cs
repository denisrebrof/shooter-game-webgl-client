using System;
using System.Collections;
using Shooter.domain.Model;
using UnityEngine;
using UnityEngine.AI;

namespace Shooter.presentation.Player.Movement
{
    public class SimplePlayerMovement : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float heightOffset;
        [SerializeField] private float unstablePingOffsetCoef = 1.3f;

        private float lastPositionTime = -0.1f;
        private Coroutine moveRoutine;

        private void OnDisable()
        {
            if (moveRoutine != null)
                StopCoroutine(moveRoutine);
        }

        public void Rewind()
        {
            lastPositionTime = -0.1f;
            if (moveRoutine != null)
                StopCoroutine(moveRoutine);
        }

        public void MoveTo(TransformSnapshot pos)
        {
            if (!isActiveAndEnabled)
                return;

            if (moveRoutine != null)
                StopCoroutine(moveRoutine);
            
            var currentTime = Time.realtimeSinceStartup;
            var duration = lastPositionTime > 0f ? currentTime - lastPositionTime : 0.0f;
            var moveEnum = MoveToPosition(pos, duration * unstablePingOffsetCoef);
            lastPositionTime = currentTime;
            moveRoutine = StartCoroutine(moveEnum);
        }

        private IEnumerator MoveToPosition(TransformSnapshot pos, float duration)
        {
            var startPosition = target.position;
            var startRotation = target.rotation;
            var targetPosition = pos.Pos + Vector3.down * heightOffset;
            var targetRotation = Quaternion.Euler(0, pos.r, 0);

            var timer = duration;
            var oneDivDuration = 1f / duration;
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                LerpTransform(1f - timer * oneDivDuration);
                yield return null;
            }

            LerpTransform(1f);

            yield break;

            void LerpTransform(float t)
            {
                target.position = Vector3.Lerp(startPosition, targetPosition, t);
                target.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            }
        }
    }
}