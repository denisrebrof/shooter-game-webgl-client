using UnityEngine;

namespace Shooter.presentation.Player.EasyFPS
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovementScript : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        private Transform playerTransform;

        [Tooltip("Current players speed")] public float currentSpeed;

        [Tooltip("Force that moves player into jump")]
        public float jumpForce = 500;

        private bool inputActive = true;

        public void SetInputActive(bool active)
        {
            inputActive = active;
        }

        /*
         * Getting the Players rigidbody component.
         * And grabbing the mainCamera from Players child transform.
         */
        private void Awake()
        {
            ignoreLayer = 1 << LayerMask.NameToLayer("Player");
            playerTransform = rb.transform;
        }

        private Vector3 slowdownV;
        private Vector2 horizontalMovement;

        //Raycasting for melee attacks and input movement handling here.
        private void FixedUpdate() => PlayerMovementLogic();

        /*
         * Accordingly to input adds force and if magnitude is bigger it will clamp it.
         * If player leaves keys it will deaccelerate
         */
        private void PlayerMovementLogic()
        {
            var currentVelocity = rb.velocity;
            currentSpeed = currentVelocity.magnitude;
            horizontalMovement = new Vector2(currentVelocity.x, currentVelocity.z);
            if (horizontalMovement.magnitude > maxSpeed)
            {
                horizontalMovement = horizontalMovement.normalized;
                horizontalMovement *= maxSpeed;
            }

            rb.velocity = new Vector3(
                horizontalMovement.x,
                currentVelocity.y,
                horizontalMovement.y
            );
            if (grounded)
            {
                rb.velocity = Vector3.SmoothDamp(
                    rb.velocity,
                    Vector3.up * currentVelocity.y,
                    ref slowdownV,
                    deaccelerationSpeed);
            }

            var movementMultiplier = accelerationSpeed * Time.deltaTime * (grounded ? 1f : 0.5f);
            var movementForceX = inputActive ? Input.GetAxis("Horizontal") * movementMultiplier : 0f;
            var movementForceY = inputActive ? Input.GetAxis("Vertical") * movementMultiplier : 0f;
            rb.AddRelativeForce(movementForceX, 0, movementForceY);

            //Slippery issues fixed here
            var hasMovementInput = movementForceX != 0 || movementForceY != 0;
            deaccelerationSpeed = hasMovementInput ? 0.5f : 0.1f;
        }

        //Handles jumping and ads the force and sounds.
        private void Jumping()
        {
            if (!inputActive || !grounded || !Input.GetKeyDown(KeyCode.Space))
                return;

            rb.AddRelativeForce(Vector3.up * jumpForce);
            jumpSound.Play();
            walkSound.Stop();
            runSound.Stop();
        }

        private void Update()
        {
            Jumping();
            Crouching();
            WalkingSound();
        }

        //Checks if player is grounded and plays the sound accordingly to his speed
        private void WalkingSound()
        {
            if (!walkSound || !runSound)
            {
                print("Missing walk and running sounds.");
                return;
            }

            if (!RayCastGrounded())
            {
                walkSound.Stop();
                runSound.Stop();
                return;
            }

            //for walk sound using this because surface is not straight			
            if (!(currentSpeed > 1))
            {
                walkSound.Stop();
                runSound.Stop();
                return;
            }

            if (maxSpeed == 3)
            {
                if (walkSound.isPlaying) return;
                walkSound.Play();
                runSound.Stop();
            }
            else if (maxSpeed == 5)
            {
                if (runSound.isPlaying) return;
                walkSound.Stop();
                runSound.Play();
            }
        }

        /*
         * Raycasts down to check if we are grounded along the gorunded method() because if the
         * floor is curvy it will go ON/OFF constatly this assures us if we are really grounded
         */
        private bool RayCastGrounded()
        {
            if (!Physics.Raycast(playerTransform.position, playerTransform.up * -1f, out var groundedInfo, 1,
                    ~ignoreLayer))
                return false;

            Debug.DrawRay(playerTransform.position, playerTransform.up * -1f, Color.red, 0.0f);
            return groundedInfo.transform != null;
        }

        //If player toggle the crouch it will scale the player to appear that is crouching
        private void Crouching()
        {
            // var verticalScale = Input.GetKey(KeyCode.C) ? 0.6f : 1f;
            var verticalScale = 1f;
            var targetScale = new Vector3(1, verticalScale, 1);
            playerTransform.localScale = Vector3.Lerp(playerTransform.localScale, targetScale, Time.deltaTime * 15);
        }


        [Tooltip("The maximum speed you want to achieve")]
        public int maxSpeed = 5;

        [Tooltip("The higher the number the faster it will stop")]
        public float deaccelerationSpeed = 15.0f;


        [Tooltip("Force that is applied when moving forward or backward")]
        public float accelerationSpeed = 50000.0f;


        [Tooltip("Tells us weather the player is grounded or not.")]
        public bool grounded;

        /*
         * checks if our player is contacting the ground in the angle less than 60 degrees
         *	if it is, set grouded to true
         */
        private void OnCollisionStay(Collision other)
        {
            foreach (var contact in other.contacts)
            {
                if (Vector2.Angle(contact.normal, Vector3.up) < 60)
                {
                    grounded = true;
                }
            }
        }

        //On collision exit set grounded to false
        private void OnCollisionExit()
        {
            grounded = false;
        }

        [Header("Shooting Properties")] [Tooltip("Put 'Player' layer here")]
        private LayerMask ignoreLayer; //to ignore player layer

        [Header("Player SOUNDS")] [Tooltip("Jump sound when player jumps.")]
        public AudioSource jumpSound;

        [Tooltip("Walk sound player makes.")] public AudioSource walkSound;
        [Tooltip("Run Sound player makes.")] public AudioSource runSound;
    }
}