using UnityEngine;

namespace Shooter.presentation.Player.Weapons
{
    public class HandPoseAnimationProxy : MonoBehaviour
    {
        [SerializeField] private HandPoseController left;
        [SerializeField] private HandPoseController right;

        public void SetTargetPose(string L_RTargets)
        {
            var targets = L_RTargets.Split('_');
            left.targetPose = targets[0];
            right.targetPose = targets[1];
        }

        public void LSetTargetPose(string target)
        {
            left.targetPose = target;
        }

        public void RSetTargetPose(string target)
        {
            right.targetPose = target;
        }
    }
}