using UnityEngine;

namespace Shooter.presentation.Player.IK
{
    public class PlayerIKControl : MonoBehaviour, IIKControl
    {
        [SerializeField] private ArmIK right;
        [SerializeField] private ArmIK left;

        public void SetElbows(Transform rightElbow, Transform leftElbow)
        {
            right.SetElbow(rightElbow);
            left.SetElbow(leftElbow);
        }
        
        public void SetTargets(Transform rightHand, Transform leftHand)
        {
            right.SetTarget(rightHand);
            left.SetTarget(leftHand);
        }
    }
}