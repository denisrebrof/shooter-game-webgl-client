using UnityEngine;

namespace Utils.IK
{
    [RequireComponent(typeof(Animator))]
    public class HandsIKBinder : MonoBehaviour, IIKHandler
    {
        [SerializeField] private Transform rightHandTarget;
        [SerializeField] private Transform leftHandTarget;

        [SerializeField] private Animator animator;

        public void SetTargets(Transform l, Transform r)
        {
            rightHandTarget = r;
            leftHandTarget = l;
        }

        private void Reset() => animator = GetComponent<Animator>();

        private void OnAnimatorIK(int layerIndex)
        {
            if (rightHandTarget != null) UpdateIKTarget(AvatarIKGoal.RightHand, rightHandTarget);
            if (leftHandTarget != null) UpdateIKTarget(AvatarIKGoal.LeftHand, leftHandTarget);
        }

        private void UpdateIKTarget(AvatarIKGoal goal, Transform target)
        {
            animator.SetIKRotationWeight(goal, 1f);
            animator.SetIKRotation(goal, target.rotation);
            animator.SetIKPositionWeight(goal, 1f);
            animator.SetIKPosition(goal, target.position);
        }
    }
}