using UnityEngine;

namespace Shooter.presentation.Player.Movement
{
    public class MovementDirectionAnimationHandler : MonoBehaviour
    {
        [SerializeField] private CharacterAnimator animator;
        [SerializeField] private float damping = 0.5f;
        [SerializeField] private float minSpeed = 0.01f;
        [SerializeField] private float fullSpeed = 0.5f;

        private Vector3 velocity;
        private Vector3 prevPos;

        private float sqrMinSpeed = 1f;

        private float SqrMinSpeed
        {
            get
            {
                if (sqrMinSpeed > 0f)
                    return sqrMinSpeed;

                sqrMinSpeed = minSpeed * minSpeed;
                return sqrMinSpeed;
            }
        }

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

        private void Rebind()
        {
            prevPos = Target.position;
            velocity = Vector3.zero;
        }

        private void OnEnable() => Rebind();

        private void Update()
        {
            var currentPos = Target.position;
            var delta = currentPos - prevPos;
            var speed = delta / Time.deltaTime;
            prevPos = currentPos;
            var localSpeed = speed.sqrMagnitude > SqrMinSpeed
                ? Target.worldToLocalMatrix.MultiplyVector(speed) / fullSpeed
                : Vector3.zero;

            velocity = Vector3.Slerp(velocity, localSpeed, Time.deltaTime / damping);
            animator.direction = new Vector2(velocity.x, velocity.z);
        }
    }
}