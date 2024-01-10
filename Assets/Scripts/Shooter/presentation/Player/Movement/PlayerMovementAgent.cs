using System;
using System.Collections;
using Shooter.domain;
using Shooter.domain.Model;
using UnityEngine;
using UnityEngine.AI;

namespace Shooter.presentation.Player.Movement
{
    public class PlayerMovementAgent : MonoBehaviour
    {
        private const float DefaultSqrOffset = -1f;

        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float destinationReachedOffset = 0.1f;
        [SerializeField] private float rotationReachedOffsetAngle = 1f;
        [SerializeField] private float rotationSpeed = 1f;

        private Transform target;

        private Transform Target
        {
            get
            {
                if (target != null)
                    return target;

                target = transform;
                return target;
            }
        }

        private float sqrDestinationReachedOffset = DefaultSqrOffset;

        private float SqrDestinationReachedOffset
        {
            get
            {
                if (sqrDestinationReachedOffset > 0f)
                    return sqrDestinationReachedOffset;

                sqrDestinationReachedOffset = destinationReachedOffset * destinationReachedOffset;
                return sqrDestinationReachedOffset;
            }
        }

        private Transform viewRoot;

        private Transform ViewRoot
        {
            get
            {
                if (viewRoot != null)
                    return viewRoot;

                viewRoot = transform;
                return viewRoot;
            }
        }
        
        public void Rewind()
        {
            agent.ResetPath();
            agent.enabled = false;
            agent.enabled = true;
        }

        public void MoveTo(TransformSnapshot pos)
        {
            if (!isActiveAndEnabled)
                return;

            StopAllCoroutines();
            StartCoroutine(MoveToPosition(pos));
        }

        private void OnDisable() => StopAllCoroutines();

        private IEnumerator MoveToPosition(TransformSnapshot pos)
        {
            var targetRotation = Quaternion.Euler(0, pos.r, 0);
            
            if(RunCompleted())
                yield break;
            
            agent.destination = pos.Pos;
            while (!RunCompleted())
            {
                var t = Time.deltaTime * rotationSpeed;
                Target.rotation = Quaternion.Slerp(Target.rotation, targetRotation, t);
                yield return null;
            }

            yield break;

            bool RunCompleted()
            {
                var toPosition = pos.Pos - ViewRoot.position;
                if (toPosition.sqrMagnitude > SqrDestinationReachedOffset)
                    return false;

                var rotationDelta = ViewRoot.rotation.eulerAngles.y - pos.r;
                return Math.Abs(rotationDelta) < rotationReachedOffsetAngle;
            }
        }
    }
}