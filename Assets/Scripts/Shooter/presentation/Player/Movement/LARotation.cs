using UnityEngine;

namespace Shooter.presentation.Player.Movement
{
    public class LARotation : MonoBehaviour
    {
        public Vector3 targetRotation;
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float damping = 0.5f;

        private Quaternion currentRotation;

        private void OnEnable()
        {
            currentRotation = Quaternion.Euler(targetRotation + offset);
        }
    
        private void LateUpdate()
        {
            var targetRot = Quaternion.Euler(targetRotation + offset);
            currentRotation = Quaternion.Slerp(currentRotation, targetRot, Time.deltaTime / damping);
            target.localRotation = currentRotation;
        }
    }
}