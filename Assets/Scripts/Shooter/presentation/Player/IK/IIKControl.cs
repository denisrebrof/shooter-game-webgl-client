using UnityEngine;

namespace Shooter.presentation.Player.IK
{
    public interface IIKControl
    {
        void SetTargets(Transform rightHand, Transform leftHand);
        void SetElbows(Transform rightElbow, Transform leftElbow);
    }
}