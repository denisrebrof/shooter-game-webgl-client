using UnityEngine;

namespace Shooter.presentation.Player.Movement
{
    public class MovementDirectionAnimationHandler : MonoBehaviour
    {
        [SerializeField] private CharacterView characterView;
        [SerializeField] private float damping = 0.5f;
        [SerializeField] private float minSpeed = 0.01f;
        [SerializeField] private float fullSpeed = 0.5f;
        
        private Vector3 prevPos;

        private float sqrMinSpeed = 1f;

        private Transform target;

        private Vector3 velocity;

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

        private void Update()
        {
            var currentPos = target.position;
            var delta = currentPos - prevPos;
            var speed = delta / Time.deltaTime;
            prevPos = currentPos;
            var localSpeed = speed.sqrMagnitude > SqrMinSpeed
                ? target.worldToLocalMatrix.MultiplyVector(speed) / fullSpeed
                : Vector3.zero;

            velocity = Vector3.Slerp(velocity, localSpeed, Time.deltaTime / damping);
            characterView.SetDirection(new Vector2(velocity.x, velocity.z));
        }

        private void OnEnable()
        {
            Rebind();
        }

        private void Rebind()
        {
            target = transform;
            prevPos = target.position;
            velocity = Vector3.zero;
        }
    }
}