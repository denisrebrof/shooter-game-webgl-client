using Michsky.MUIP;
using Shooter.data;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Utils.IK;
using Utils.Reactive;
using Zenject;

namespace Shooter.presentation
{
    public class LobbyPlayerController : MonoBehaviour
    {
        [Inject] private IWeaponStoreRepository weaponStoreRepository;
        [Inject] private WeaponDataSO weaponData;
        [SerializeField] private Transform weaponRoot;
        [SerializeField] private GameObject soviet;
        [SerializeField] private HandsIKBinder sovietBinder;
        [SerializeField] private GameObject german;
        [SerializeField] private HandsIKBinder germanBinder;
        [SerializeField] private ButtonManager sovietButton;
        [SerializeField] private ButtonManager germanButton;

        private bool isSoviet = true;

        private PlayerIKTargets targets;

        private void Start()
        {
            sovietButton.Interactable(!isSoviet);
            germanButton.Interactable(isSoviet);
            sovietButton.onClick.AddListener(() => SetIsSoviet(true));
            germanButton.onClick.AddListener(() => SetIsSoviet(false));
            weaponStoreRepository
                .GetWeaponsData()
                .Select(data => data.primaryId)
                .DistinctUntilChanged()
                .Select(weaponData.GetData)
                .Select(data => data.lobbyPrefab.LoadPrefabObservable())
                .Switch()
                .Subscribe(SpawnPlayerWeapon)
                .AddTo(this);
        }

        [ContextMenu("Soviet")]
        public void SetS() => SetIsSoviet(true);
        
        [ContextMenu("German")]
        public void SetG() => SetIsSoviet(false);

        public void SetIsSoviet(bool isSoviet)
        {
            this.isSoviet = isSoviet;
            sovietButton.Interactable(!isSoviet);
            germanButton.Interactable(isSoviet);
            UpdateBinding();
        }

        private void UpdateBinding()
        {
            german.SetActive(!isSoviet);
            soviet.SetActive(isSoviet);
            if (targets == null)
                return;

            var binder = isSoviet ? sovietBinder : germanBinder;
            binder.SetTargets(targets.left, targets.right);
        }

        private void SpawnPlayerWeapon(GameObject weapon)
        {
            for (var i = weaponRoot.childCount - 1; i >= 0; i--)
            {
                var child = weaponRoot.GetChild(i);
                Destroy(child.gameObject);
            }

            targets = Instantiate(weapon, weaponRoot).GetComponent<PlayerIKTargets>();
            UpdateBinding();
        }
    }
}