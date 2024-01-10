using UnityEngine;

namespace Shooter.presentation.Player.IK
{
    public class PlayerIKTargets : MonoBehaviour
    {
        [SerializeField] private Transform right;
        [SerializeField] private Transform left;
        [SerializeField] private Transform rightElbow;
        [SerializeField] private Transform leftElbow;

        private void OnEnable()
        {
            var ikControl = GetComponentInParent<IIKControl>();
            if (ikControl == null)
                return;

            ikControl.SetTargets(right, left);
            ikControl.SetElbows(rightElbow, leftElbow);
        }
    }
}