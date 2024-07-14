using UnityEngine;

namespace Shooter.presentation.Player
{
    public class PlayerWeaponSpawner : MonoBehaviour
    {
        [SerializeField] private Transform inventoryRoot;

        private void Awake()
        {
            InstantiateWeapon(PlayerDataStorage.PrimaryWeaponPrefab);
            InstantiateWeapon(PlayerDataStorage.SecondaryWeaponPrefab);
        }

        private void InstantiateWeapon(GameObject weapon)
        {
            weapon.SetActive(false);
            Instantiate(weapon, inventoryRoot);
        }
    }
}