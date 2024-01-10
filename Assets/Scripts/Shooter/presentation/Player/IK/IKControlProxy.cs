using UnityEngine;

namespace Shooter.presentation.Player.IK
{
    public class IKControlProxy : MonoBehaviour, IIKControl
    {
        [SerializeField] private PlayerIKControl target;

        public void SetTargets(Transform rightHand, Transform leftHand) => target.SetTargets(rightHand, leftHand);

        public void SetElbows(Transform rightElbow, Transform leftElbow) => target.SetElbows(rightElbow, leftElbow);
    }
}