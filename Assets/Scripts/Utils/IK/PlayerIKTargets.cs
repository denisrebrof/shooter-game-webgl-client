using UnityEngine;

namespace Utils.IK
{
    public class PlayerIKTargets : MonoBehaviour
    {
        public Transform right;
        public Transform left;

        private void OnEnable()
        {
            GetComponentInParent<IIKHandler>()?.SetTargets(left, right);
        }
    }
}