using System;
using System.Collections;
using Shooter.domain;
using Shooter.presentation.Player.Weapons;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Shooter.presentation.Player.EasyFPS
{
    public enum GunStyles
    {
        NonAutomatic,
        Automatic
    }

    public class GunScript : MonoBehaviour
    {
        [Inject] private CrosshairUIController crosshairUIController;
        [Inject] private SimplePlayerBulletPool bulletsPool;
        [Inject] private ShootPlayerUseCase shootPlayerUseCase;
        [Inject(Id = "PlayerCanvas")] private Canvas playerCanvas;

        [Tooltip("Selects type of weapon to shoot rapidly or one bullet per click.")]
        public GunStyles currentStyle;

        [HideInInspector] public MouseLookScript mls;

        [Header("Player movement properties")]
        [Tooltip(
            "Speed is determined via gun because not every gun has same properties or weights so you MUST set up your speeds here")]
        public int walkingSpeed = 3;

        [Tooltip(
            "Speed is determined via gun because not every gun has same properties or weights so you MUST set up your speeds here")]
        public int runningSpeed = 5;


        [Header("Bullet properties")]
        [Tooltip("Preset value to tell with how many bullets will our weapon spawn aside.")]
        public float bulletsIHave = 20;

        [Tooltip("Preset value to tell with how much bullets will our weapon spawn inside rifle.")]
        public float bulletsInTheGun = 5;

        [Tooltip("Preset value to tell how much bullets can one magazine carry.")]
        public float amountOfBulletsPerLoad = 5;

        private Transform player;
        private Rigidbody playerRb;
        private PlayerMovementScript pmS;

        private UnityEngine.Camera cameraComponent;
        private Transform gunPlaceHolder;


        public long weaponId;

        /*
         * Collection the variables upon awake that we need.
         */
        private void Awake()
        {
            mls = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLookScript>();
            player = mls.transform;
            playerRb = mls.gameObject.GetComponent<Rigidbody>();
            mainCamera = mls.myCamera;
            cameraComponent = mainCamera.GetComponent<UnityEngine.Camera>();
            pmS = player.GetComponent<PlayerMovementScript>();

            rotationLastY = mls.currentYRotation;
            rotationLastX = mls.currentCameraXRotation;
        }


        [HideInInspector] public Vector3 currentGunPosition;

        [Header("Gun Positioning")] [Tooltip("Vector 3 position from player SETUP for NON AIMING values")]
        public Vector3 restPlacePosition;

        [Tooltip("Vector 3 position from player SETUP for AIMING values")]
        public Vector3 aimPlacePosition;

        [Tooltip("Time that takes for gun to get into aiming stance.")]
        public float gunAimTime = 0.1f;

        [HideInInspector] public bool reloading;

        private Vector3 gunPosVelocity;
        private float cameraZoomVelocity;

        private Vector2 gunFollowTimeVelocity;

        private bool inputActive = true;

        public void SetInputActive(bool active)
        {
            inputActive = active;
        }

        /*
    Update loop calling for methods that are described below where they are initiated.
    */
        private void Update()
        {
            Animations();
            GiveCameraScriptMySensitivity();
            PositionGun();
            Shooting();
            Sprint(); //iff we have the gun you sprint from here, if we are gunless then its called from movement script
            crosshairUIController.CrossHairExpansionWhenWalking(
                Input.GetAxis("Fire1") != 0 && Cursor.lockState == CursorLockMode.Locked,
                playerRb.velocity.magnitude > 1,
                pmS.maxSpeed >= runningSpeed,
                Input.GetAxis("Fire2") != 0 && !reloading && Cursor.lockState == CursorLockMode.Locked
            );
        }

        /*
         *Update loop calling for methods that are described below where they are initiated.
         *+
         *Calculation of weapon position when aiming or not aiming.
         */
        private void FixedUpdate()
        {
            RotationGun();

            /*
             * Changing some values if we are aiming, like sensitivity, zoom ratio and position of the weapon.
             */
            //if aiming
            if (inputActive && Input.GetAxis("Fire2") != 0 && !reloading)
            {
                gunPrecision = gunPrecision_aiming;
                recoilAmount_x = recoilAmount_x_;
                recoilAmount_y = recoilAmount_y_;
                recoilAmount_z = recoilAmount_z_;
                currentGunPosition =
                    Vector3.SmoothDamp(currentGunPosition, aimPlacePosition, ref gunPosVelocity, gunAimTime);
                cameraComponent.fieldOfView = Mathf.SmoothDamp(cameraComponent.fieldOfView, cameraZoomRatio_aiming,
                    ref cameraZoomVelocity, gunAimTime);
            }
            //if not aiming
            else
            {
                gunPrecision = gunPrecision_notAiming;
                recoilAmount_x = recoilAmount_x_non;
                recoilAmount_y = recoilAmount_y_non;
                recoilAmount_z = recoilAmount_z_non;
                currentGunPosition =
                    Vector3.SmoothDamp(currentGunPosition, restPlacePosition, ref gunPosVelocity, gunAimTime);
                cameraComponent.fieldOfView = Mathf.SmoothDamp(cameraComponent.fieldOfView, cameraZoomRatio_notAiming,
                    ref cameraZoomVelocity, gunAimTime);
            }
        }

        [Header("Sensitivity of the gun")] [Tooltip("Sensitivity of this gun while not aiming.")]
        public float mouseSensitvity_notAiming = 10;

        //[HideInInspector]
        [Tooltip("Sensitivity of this gun while aiming.")]
        public float mouseSensitvity_aiming = 5;

        //[HideInInspector]
        [Tooltip("Sensitivity of this gun while running.")]
        public float mouseSensitvity_running = 4;

        /*
         * Used to give our main camera different sensitivity options for each gun.
         */
        private void GiveCameraScriptMySensitivity()
        {
            mls.mouseSensitivityNotAiming = mouseSensitvity_notAiming;
            mls.mouseSensitivityAiming = mouseSensitvity_aiming;
        }

        /*
         * Changes the max speed that player is allowed to go.
         * Also max speed is connected to the animator which will trigger the run animation.
         */
        private void Sprint()
        {
            // Running();  so i can find it with CTRL + F
            if (!(Input.GetAxis("Vertical") > 0) || Input.GetAxisRaw("Fire2") != 0 || Input.GetAxisRaw("Fire1") != 0)
            {
                pmS.maxSpeed = walkingSpeed;
                return;
            }

            if (!Input.GetKeyDown(KeyCode.LeftShift))
                return;

            //sets player movement peed to max
            pmS.maxSpeed = pmS.maxSpeed == walkingSpeed ? runningSpeed : walkingSpeed;
        }

        private Vector3 velV;
        [HideInInspector] public Transform mainCamera;

        /*
         * Calculation the weapon position accordingly to the player position and rotation.
         * After calculation the recoil amount are decreased to 0.
         */
        private void PositionGun()
        {
            transform.position = Vector3.SmoothDamp(transform.position,
                mainCamera.position -
                mainCamera.right * (currentGunPosition.x + currentRecoilXPos) +
                mainCamera.up * (currentGunPosition.y + currentRecoilYPos) +
                mainCamera.forward * (currentGunPosition.z + currentRecoilZPos), ref velV, 0);

            currentRecoilZPos = Mathf.SmoothDamp(currentRecoilZPos, 0, ref velocity_z_recoil, recoilOverTime_z);
            currentRecoilXPos = Mathf.SmoothDamp(currentRecoilXPos, 0, ref velocity_x_recoil, recoilOverTime_x);
            currentRecoilYPos = Mathf.SmoothDamp(currentRecoilYPos, 0, ref velocity_y_recoil, recoilOverTime_y);
        }


        [Header("Rotation")] private Vector2 velocityGunRotate;
        private float gunWeightX, gunWeightY;

        [Tooltip("The time weapon will lag behind the camera view best set to '0'.")]
        public float rotationLagTime;

        private float rotationLastY;
        private float rotationDeltaY;
        private float angularVelocityY;
        private float rotationLastX;
        private float rotationDeltaX;
        private float angularVelocityX;

        [Tooltip("Value of forward rotation multiplier.")]
        public Vector2 forwardRotationAmount = Vector2.one;

        /*
         * Rotate the weapon according to mouse look rotation.
         * Calculating the forward rotation like in Call Of Duty weapon weight
         */
        private void RotationGun()
        {
            rotationDeltaY = mls.currentYRotation - rotationLastY;
            rotationDeltaX = mls.currentCameraXRotation - rotationLastX;

            rotationLastY = mls.currentYRotation;
            rotationLastX = mls.currentCameraXRotation;

            angularVelocityY = Mathf.Lerp(angularVelocityY, rotationDeltaY, Time.deltaTime * 5);
            angularVelocityX = Mathf.Lerp(angularVelocityX, rotationDeltaX, Time.deltaTime * 5);

            gunWeightX = Mathf.SmoothDamp(gunWeightX, mls.currentCameraXRotation, ref velocityGunRotate.x,
                rotationLagTime);
            gunWeightY = Mathf.SmoothDamp(gunWeightY, mls.currentYRotation, ref velocityGunRotate.y, rotationLagTime);

            transform.rotation = Quaternion.Euler(gunWeightX + (angularVelocityX * forwardRotationAmount.x),
                gunWeightY + (angularVelocityY * forwardRotationAmount.y), 0);
        }

        private float currentRecoilZPos;
        private float currentRecoilXPos;

        private float currentRecoilYPos;

        /*
         * Called from ShootMethod();, upon shooting the recoil amount will increase.
         */
        private void RecoilMath()
        {
            currentRecoilZPos -= recoilAmount_z;
            currentRecoilXPos -= (Random.value - 0.5f) * recoilAmount_x;
            currentRecoilYPos -= (Random.value - 0.5f) * recoilAmount_y;
            mls.wantedCameraXRotation -= Mathf.Abs(currentRecoilYPos * gunPrecision);
            mls.wantedYRotation -= (currentRecoilXPos * gunPrecision);

            crosshairUIController.expandValuesCrosshair += new Vector2(40, 40);
        }

        [SerializeField] public Transform bulletSpawnPlace;

        [Tooltip("Rounds per second if weapon is set to automatic rafal.")]
        public float roundsPerSecond;

        private float waitTillNextFire;

        //Checking if the gun is automatic or non automatic and accordingly runs the ShootMethod();.
        private void Shooting()
        {
            if (!inputActive || Cursor.lockState != CursorLockMode.Locked)
                return;

            var isSingleFiring = currentStyle == GunStyles.NonAutomatic && Input.GetButtonDown("Fire1");
            var isAutoFiring = currentStyle == GunStyles.Automatic && Input.GetButton("Fire1");
            if (isSingleFiring || isAutoFiring)
                ShootMethod();

            waitTillNextFire -= roundsPerSecond * Time.deltaTime;
        }

        [HideInInspector] public float recoilAmount_z = 0.5f;
        [HideInInspector] public float recoilAmount_x = 0.5f;
        [HideInInspector] public float recoilAmount_y = 0.5f;

        [Header("Recoil Not Aiming")] [Tooltip("Recoil amount on that AXIS while NOT aiming")]
        public float recoilAmount_z_non = 0.5f;

        [Tooltip("Recoil amount on that AXIS while NOT aiming")]
        public float recoilAmount_x_non = 0.5f;

        [Tooltip("Recoil amount on that AXIS while NOT aiming")]
        public float recoilAmount_y_non = 0.5f;

        [Header("Recoil Aiming")] [Tooltip("Recoil amount on that AXIS while aiming")]
        public float recoilAmount_z_ = 0.5f;

        [Tooltip("Recoil amount on that AXIS while aiming")]
        public float recoilAmount_x_ = 0.5f;

        [Tooltip("Recoil amount on that AXIS while aiming")]
        public float recoilAmount_y_ = 0.5f;

        [HideInInspector] public float velocity_z_recoil, velocity_x_recoil, velocity_y_recoil;

        [Header("")]
        [Tooltip(
            "The time that takes weapon to get back on its original axis after recoil.(The smaller number the faster it gets back to original position)")]
        public float recoilOverTime_z = 0.5f;

        [Tooltip(
            "The time that takes weapon to get back on its original axis after recoil.(The smaller number the faster it gets back to original position)")]
        public float recoilOverTime_x = 0.5f;

        [Tooltip(
            "The time that takes weapon to get back on its original axis after recoil.(The smaller number the faster it gets back to original position)")]
        public float recoilOverTime_y = 0.5f;

        [Header("Gun Precision")]
        [Tooltip("Gun rate precision when player is not aiming. THis is calculated with recoil.")]
        public float gunPrecision_notAiming = 200.0f;

        [Tooltip("Gun rate precision when player is aiming. THis is calculated with recoil.")]
        public float gunPrecision_aiming = 100.0f;

        [Tooltip("FOV of first camera when NOT aiming(ONLY SECOND CAMERA RENDERS WEAPONS")]
        public float cameraZoomRatio_notAiming = 60;

        [Tooltip("FOV of first camera when aiming(ONLY SECOND CAMERA RENDERS WEAPONS")]
        public float cameraZoomRatio_aiming = 40;

        [HideInInspector] public float gunPrecision;

        [Tooltip("Audios for shootingSound, and reloading.")]
        public AudioSource shoot_sound_source, reloadSound_source;

        [Tooltip("Array of muzzel flashes, randmly one will appear after each bullet.")]
        public GameObject[] muzzelFlash;

        public Light muzzelLight;

        /*
         * Called from Shooting();
         * Creates bullets and muzzle flashes and calls for Recoil.
         */
        private void ShootMethod()
        {
            if (!(waitTillNextFire <= 0) || reloading || pmS.maxSpeed >= 5)
                return;

            if (!(bulletsInTheGun > 0))
            {
                StartCoroutine(Reload_Animation());
                return;
            }

            var spawnPos = bulletSpawnPlace.position;
            var spawnedBullet = bulletsPool.Pop();
            spawnedBullet.weaponId = weaponId;
            spawnedBullet.ReturnToPool = () => bulletsPool.Return(spawnedBullet);
            spawnedBullet.Reset(spawnPos, bulletSpawnPlace.rotation);

            shootPlayerUseCase.Shoot(spawnPos, bulletSpawnPlace.forward, weaponId);

            StartCoroutine(FlashEnumerator());

            if (shoot_sound_source)
                shoot_sound_source.Play();
            else
                print("Missing 'Shoot Sound Source'.");

            RecoilMath();

            waitTillNextFire = 1;
            bulletsInTheGun -= 1;
        }

        private IEnumerator FlashEnumerator()
        {
            var randomNumberForMuzzleFlash = Random.Range(0, 5);
            var flash = muzzelFlash[randomNumberForMuzzleFlash];
            flash.SetActive(true);
            muzzelLight.enabled = true;
            muzzelLight.intensity += 1f;
            yield return new WaitForSeconds(0.05f);
            var intensity = muzzelLight.intensity - 1;
            muzzelLight.intensity = intensity;
            muzzelLight.enabled = intensity > 0.1f;
            flash.SetActive(false);
        }

        private void OnEnable()
        {
            foreach (var flash in muzzelFlash)
                flash.SetActive(false);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            foreach (var flash in muzzelFlash)
                flash.SetActive(false);

            muzzelLight.intensity = 0f;
            muzzelLight.enabled = false;
        }


        /*
         * Reloading, setting the reloading to animator,
         * Waiting for 2 seconds and then setting the reloaded clip.
         */
        [Header("reload time after anima")]
        [Tooltip(
            "Time that passes after reloading. Depends on your reload animation length, because reloading can be interrupted via meele attack or running. So any action before this finishes will interrupt reloading.")]
        public float reloadChangeBulletsTime;

        private IEnumerator Reload_Animation()
        {
            if (!(bulletsIHave > 0) || !(bulletsInTheGun < amountOfBulletsPerLoad) || reloading) yield break;
            if (reloadSound_source.isPlaying == false && reloadSound_source != null)
            {
                if (reloadSound_source)
                    reloadSound_source.Play();
                else
                    print("'Reload Sound Source' missing.");
            }
            
            handsAnimator.SetBool("reloading", true);
            yield return new WaitForSeconds(0.5f);
            handsAnimator.SetBool("reloading", false);
            
            yield return new WaitForSeconds(reloadChangeBulletsTime - 0.5f); //minus ovo vrijeme cekanja na yield
            if (pmS.maxSpeed == runningSpeed) yield break;

            if (bulletsIHave - amountOfBulletsPerLoad >= 0)
            {
                bulletsIHave -= amountOfBulletsPerLoad - bulletsInTheGun;
                bulletsInTheGun = amountOfBulletsPerLoad;
                yield break;
            }

            var valueForBoth = amountOfBulletsPerLoad - bulletsInTheGun;
            if (bulletsIHave - valueForBoth < 0)
            {
                bulletsInTheGun += bulletsIHave;
                bulletsIHave = 0;
            }
            else
            {
                bulletsIHave -= valueForBoth;
                bulletsInTheGun += valueForBoth;
            }
        }

        /*
         * Setting the number of bullets to the hud UI gameobject if there is one.
         * And drawing CrossHair from here.
         */
        [Tooltip("HUD bullets to display bullet count on screen. Will be find under name 'HUD_bullets' in scene.")]
        public TMP_Text HUD_bullets;

        private void OnGUI()
        {
            if (!playerCanvas.enabled)
                return;

            if (!HUD_bullets)
            {
                try
                {
                    HUD_bullets = GameObject.Find("HUD_bullets").GetComponent<TMP_Text>();
                }
                catch (Exception ex)
                {
                    print("Couldn't find the HUD_Bullets ->" + ex.StackTrace);
                }
            }

            if (mls && HUD_bullets)
                HUD_bullets.text = bulletsIHave + " - " + bulletsInTheGun;
        }

        public Animator handsAnimator;

        /*
         * Fetching if any current animation is running.
         * Setting the reload animation upon pressing R.
         */
        private void Animations()
        {
            if (!handsAnimator) return;
            reloading = handsAnimator.GetCurrentAnimatorStateInfo(0).IsName(reloadAnimationName);

            handsAnimator.SetFloat("walkSpeed", pmS.currentSpeed);
            handsAnimator.SetBool("aiming", Input.GetButton("Fire2"));
            handsAnimator.SetInteger("maxSpeed", pmS.maxSpeed);
            if (inputActive && Input.GetKeyDown(KeyCode.R) && pmS.maxSpeed < 5 && !reloading /* && !aiming*/)
            {
                StartCoroutine(nameof(Reload_Animation));
            }
        }

        [Header("Animation names")] public string reloadAnimationName = "Player_Reload";
    }
}