using UnityEngine;

namespace Shooter.presentation.Player.EasyFPS
{
    public class MouseLookScript : MonoBehaviour
    {
        [SerializeField] public Transform myCamera;
        [SerializeField] public PlayerMovementScript movement;

        private void OnEnable()
        {
            var currentY = transform.rotation.eulerAngles.y;
            wantedYRotation = currentY;
            currentYRotation = currentY;
        }

        //Triggering the headbob camera movement if player is faster than 1 of speed
        private void Update()
        {
            MouseInputMovement();
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            if (movement.currentSpeed > 1)
                HeadMovement();
        }

        [Header("Z Rotation Camera")] [HideInInspector]
        public float timer;

        [HideInInspector] public int int_timer;
        [HideInInspector] public float zRotation;
        [HideInInspector] public float wantedZ;
        [HideInInspector] public float timeSpeed = 2;

        [HideInInspector] public float timerToRotateZ;

        /*
         * Switching Z rotation and applying to camera in camera Rotation().
         */
        private void HeadMovement()
        {
            timer += timeSpeed * Time.deltaTime;
            int_timer = Mathf.RoundToInt(timer);
            wantedZ = int_timer % 2 == 0 ? -1 : 1;
            zRotation = Mathf.Lerp(zRotation, wantedZ, Time.deltaTime * timerToRotateZ);
        }

        [Tooltip("Current mouse sensitivity, changes in the weapon properties")]
        public float mouseSensitivity = 0;

        [HideInInspector] public float mouseSensitivityNotAiming = 300;
        [HideInInspector] public float mouseSensitivityAiming = 50;

        //If aiming set the mouse sensitivity from our variables and vice versa.
        private void FixedUpdate()
        {
            mouseSensitivity = Input.GetAxis("Fire2") != 0 ? mouseSensitivityAiming : mouseSensitivityNotAiming;
            ApplyingStuff();
        }


        private float rotationYVelocity, cameraXVelocity;

        [Tooltip("Speed that determines how much camera rotation will lag behind mouse movement.")]
        public float yRotationSpeed, xCameraSpeed;

        [HideInInspector] public float wantedYRotation;
        [HideInInspector] public float currentYRotation;

        [HideInInspector] public float wantedCameraXRotation;
        [HideInInspector] public float currentCameraXRotation;

        [Tooltip("Top camera angle.")] public float topAngleView = 60;

        [Tooltip("Minimum camera angle.")] public float bottomAngleView = -45;

/*
 * Upon mouse movenet it increases/decreased wanted value. (not actually moving yet)
 * Clamping the camera rotation X to top and bottom angles.
 */
        void MouseInputMovement()
        {
            wantedYRotation += Input.GetAxis("Mouse X") * mouseSensitivity;
            wantedCameraXRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            wantedCameraXRotation = Mathf.Clamp(wantedCameraXRotation, bottomAngleView, topAngleView);
        }

/*
 * Smoothing the wanted movement.
 * Calling the waeponRotation form here, we are rotating the waepon from this script.
 * Applying the camera wanted rotation to its transform.
 */
        void ApplyingStuff()
        {
            currentYRotation =
                Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, yRotationSpeed);
            currentCameraXRotation = Mathf.SmoothDamp(currentCameraXRotation, wantedCameraXRotation,
                ref cameraXVelocity,
                xCameraSpeed);

            transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
            myCamera.localRotation = Quaternion.Euler(currentCameraXRotation, 0, zRotation);
        }

        private Vector2 velocityGunFollow;
        private float gunWeightX, gunWeightY;

        float deltaTime = 0.0f;

        [Tooltip("Shows FPS in top left corner.")]
        public bool showFps = true;

        //Shows fps if its set to true.
        private void OnGUI()
        {
            if (showFps)
            {
                FPSCounter();
            }
        }

        //Calculating real fps because unity status tab shows too much fps even when its not that mutch so i made my own.
        private void FPSCounter()
        {
            int w = Screen.width, h = Screen.height;

            var style = new GUIStyle();

            var rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = Color.white;
            var msec = deltaTime * 1000.0f;
            var fps = 1.0f / deltaTime;
            var text = $"{msec:0.0} ms ({fps:0.} fps)";
            GUI.Label(rect, text, style);
        }
    }
}