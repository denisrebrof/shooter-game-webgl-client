using System.Collections;
using Plugins.GoogleAnalytics;
using Shooter.domain;
using Shooter.presentation.UI;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Player.EasyFPS
{
    public class GunInventory : MonoBehaviour
    {
        [Inject] private SelectWeaponUseCase selectWeaponUseCase;
        [Inject] private WeaponDataSO weaponDataSo;
        [Inject] private WeaponsPanelController weaponsPanelController;

        [SerializeField] private int weaponsCount;

        [Tooltip("Current weapon gameObject.")]
        public GameObject currentGun;

        private GunScript currentGunScript;

        private Animator currentHAndsAnimator;
        
        private int currentGunCounter = 0;

        [HideInInspector] public float switchWeaponCooldown;

        /*
         * Calling the method that will update the icons of our guns if we carry any upon start.
         * Also will spawn a weapon upon start.
         */
        private void Awake()
        {
            StartCoroutine(nameof(SpawnWeaponUponStart)); //to start with a gun
        }

        //Waits some time then calls for a weapon spawn
        private IEnumerator SpawnWeaponUponStart()
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(nameof(Spawn), 0);
        }

        private void OnEnable()
        {
            if (currentGunScript != null)
            {
                currentGunScript.SetInputActive(true);
            }
        }

        private void OnDisable()
        {
            if (currentGunScript != null)
            {
                currentGunScript.SetInputActive(false);
            }
        }

        /*
         * Calculation switchWeaponCoolDown so it does not allow us to change weapons millions of times per second,
         * and at some point we will change the switchWeaponCoolDown to a negative value so we have to wait until it
         * overcomes 0.0f.
         */
        private void Update()
        {
            switchWeaponCooldown += 1 * Time.deltaTime;
            if (switchWeaponCooldown > 1.2f && Input.GetKey(KeyCode.LeftShift) == false)
            {
                Create_Weapon();
            }
        }

        /*
         * If used scroll mousewheel or arrows up and down the player will change weapon.
         * GunPlaceSpawner is child of Player gameObject, where the gun is going to spawn and transition to our
         * gun properties value.
         */
        private void Create_Weapon()
        {
            //Scrolling wheel weapons changing
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                switchWeaponCooldown = 0;
                currentGunCounter = ++currentGunCounter % weaponsCount;
                StartCoroutine(Spawn(currentGunCounter));
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                switchWeaponCooldown = 0;
                currentGunCounter = currentGunCounter > 0 ? currentGunCounter - 1 : weaponsCount - 1;
                StartCoroutine(Spawn(currentGunCounter));
            }

            /*
             * Keypad numbers
             */
            if (Input.GetKeyDown(KeyCode.Alpha1) && currentGunCounter != 0)
            {
                switchWeaponCooldown = 0;
                currentGunCounter = 0;
                StartCoroutine(nameof(Spawn), currentGunCounter);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && currentGunCounter != 1)
            {
                switchWeaponCooldown = 0;
                currentGunCounter = 1;
                StartCoroutine(nameof(Spawn), currentGunCounter);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha3) && currentGunCounter != 2)
            {
                switchWeaponCooldown = 0;
                currentGunCounter = 2;
                StartCoroutine(nameof(Spawn), currentGunCounter);
            }
        }

        /*
         * This method is called from Create_Weapon() upon pressing arrow up/down or scrolling the mouse wheel,
         * It will check if we carry a gun and destroy it, and its then going to load a gun prefab from our Resources Folder.
         */
        private IEnumerator Spawn(int _redniBroj)
        {
            if (weaponChanging)
                weaponChanging.Play();
            else
                print("Missing Weapon Changing music clip.");

            if (currentGun)
            {
                currentHAndsAnimator.SetBool("changingWeapon", true);
                //0.8 time to change weapon, but since there is no change weapon animation there is no need to wait fo weapon taken down
                yield return new WaitForSeconds(0.8f);
                Destroy(currentGun);
            }

            var resource = weaponDataSo.GetData(_redniBroj).playerPrefab;
            currentGun = Instantiate(resource, transform.position, Quaternion.identity);
            currentGunScript = currentGun.GetComponent<GunScript>();
            currentGunScript.weaponId = _redniBroj;
            currentHAndsAnimator = currentGunScript.handsAnimator;
            selectWeaponUseCase.Select(_redniBroj);
            weaponsPanelController.SetActiveWeapon(_redniBroj);
            GoogleAnalyticsSDK.SendNumEvent("set_weapon", "weapon_id", _redniBroj);
        }
        /*
         * Sounds
         */
        [Header("Sounds")] [Tooltip("Sound of weapon changing.")]
        public AudioSource weaponChanging;
    }
}