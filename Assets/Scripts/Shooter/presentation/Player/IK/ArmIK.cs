using UnityEngine;

namespace Shooter.presentation.Player.IK
{
    [ExecuteInEditMode]
    public class ArmIK : MonoBehaviour
    {
        [SerializeField] private bool apply;

        [SerializeField] private Transform upperArm;
        [SerializeField] private Transform foreArm;
        [SerializeField] private Transform hand;

        [SerializeField] private Transform target;
        [SerializeField] private Transform elbow;

        private bool lengthCalculated;
        private float upperArmLength;
        private float foreArmLength;
        private float maxArmLength;

        public void SetTarget(Transform newTarget) => target = newTarget;

        public void SetElbow(Transform newElbow) => elbow = newElbow;

        private void Awake() => lengthCalculated = false;

        private void LateUpdate()
        {
            if (!apply || target == null)
                return;

            var upperArmPos = upperArm.position;
            var foreArmPos = foreArm.position;

            if (!lengthCalculated)
            {
                upperArmLength = (upperArmPos - foreArmPos).magnitude;
                foreArmLength = (foreArmPos - hand.position).magnitude;
                maxArmLength = upperArmLength + foreArmLength;
                maxArmLength *= 0.95f; // Floating point fix
            }

            var toTarget = target.position - upperArmPos;
            var distance = toTarget.magnitude;
            toTarget = toTarget.normalized * Mathf.Min(maxArmLength, distance);
            distance = toTarget.magnitude;

            var elbowOffset = CalculateElbowOffset(upperArmLength, foreArmLength, distance);
            if (float.IsNaN(elbowOffset)) // stub
                return;

            var elbowProjectionOffset = Mathf.Sqrt(upperArmLength * upperArmLength - elbowOffset * elbowOffset);
            var elbowProjectionPoint = upperArmPos + elbowProjectionOffset * toTarget.normalized;
            var elbowDirPos = elbow == null ? foreArmPos : elbow.position;
            var prevElbowVector = elbowDirPos - upperArmPos - Vector3.Project(elbowDirPos - upperArmPos, toTarget);
            var elbowVector = prevElbowVector.normalized * elbowOffset;

            var frPos = elbowProjectionPoint + elbowVector;
            var nandPos = upperArmPos + toTarget.normalized * Mathf.Min(maxArmLength, distance);

            RotateTo(upperArm, frPos);
            RotateTo(foreArm, nandPos);
            hand.rotation = target.rotation;

            if (float.IsNaN(frPos.x)) // stub
                return;
            
            foreArm.position = frPos;
            hand.position = nandPos;
        }

        private static void RotateTo(Transform source, Vector3 pos)
        {
            source.LookAt(pos);
            source.Rotate(Vector3.right * 90f);
        }

        private static float CalculateElbowOffset(
            float upperArmLength,
            float foreArmLength,
            float distance
        )
        {
            var perimeter = upperArmLength + foreArmLength + distance;
            var p = perimeter / 2f;
            var rootContent = p * (p - distance) * (p - foreArmLength) * (p - upperArmLength);
            rootContent = Mathf.Abs(rootContent);
            return Mathf.Sqrt(rootContent) * 2f / distance;
        }
    }
}