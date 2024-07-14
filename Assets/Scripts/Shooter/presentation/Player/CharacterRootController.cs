using System;
using System.Collections.Generic;
using Shooter.data;
using Shooter.domain.Model;
using Shooter.presentation.Player.Movement;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils.Reactive;
using Zenject;

namespace Shooter.presentation.Player
{
    public class CharacterRootController : PlayerDataBehaviour
    {
        [Inject] private WeaponDataSO weaponData;

        [SerializeField] private SimplePlayerMovement movement;
        [SerializeField] private CharacterView characterViewRed;
        [SerializeField] private CharacterView characterViewBlue;
        [SerializeField] private Collider hitbox;

        [Header("Movement Directions Anim Sync")]

        [SerializeField] private float damping = 0.5f;
        [SerializeField] private float minSpeed = 0.01f;
        [SerializeField] private float fullSpeed = 0.5f;

        [Header("Debug Info")]

        [SerializeField] private bool drawDebugPositions;
        [SerializeField] private bool crouching;

        private IDisposable handler = Disposable.Empty;
        private IDisposable setWeaponHandler = Disposable.Empty;

        private readonly List<Vector3> debugPositionSnapshots = new();

        private Transform target;
        private Vector3 prevPos;
        private Vector3 velocity;
        private bool posInitialized;

        private long selectedWeaponId = -1;
        private bool alive = true;

        private CharacterView characterView;

        private float sqrMinSpeed = -1f;
        private float oneDivFullSpeed;

        private float SqrMinSpeed =>
            sqrMinSpeed > 0f
                ? sqrMinSpeed
                : sqrMinSpeed = minSpeed * minSpeed;

        private void OnEnable()
        {
            target = transform;
            prevPos = target.position;
            velocity = Vector3.zero;
            posInitialized = false;
            oneDivFullSpeed = 1f / fullSpeed;
            handler = PlayerDataFlow.Subscribe(UpdateData).AddTo(this);
        }

        private void OnDisable()
        {
            handler.Dispose();
            setWeaponHandler.Dispose();
        }

        private void Update()
        {
            UpdateAnimations();
        }

        private void UpdateAnimations()
        {
            var currentPos = target.position;
            var delta = currentPos - prevPos;
            var speed = delta / Time.deltaTime;
            prevPos = currentPos;
            var localSpeed = speed.sqrMagnitude > SqrMinSpeed
                ? target.worldToLocalMatrix.MultiplyVector(speed) * oneDivFullSpeed
                : Vector3.zero;

            velocity = Vector3.Slerp(velocity, localSpeed, Time.deltaTime / damping);
            characterView?.SetDirection(new Vector2(velocity.x, velocity.z));
        }

        private void OnDrawGizmos() => DrawDebugPositions();

        private void DrawDebugPositions()
        {
            if (!drawDebugPositions)
                return;

            foreach (var pos in debugPositionSnapshots)
                Gizmos.DrawSphere(pos, 0.5f);
        }

        private void UpdateData(PlayerData data)
        {
            UpdateCharacter(data.Team);
            characterView.SetCrouching(data.crouching);
            characterView.SetJumping(data.jumping);
            UpdateWeapon(data.selectedWeaponId);
            UpdateAliveState(data.alive);
            
            if (data.pos.Pos.sqrMagnitude < 0.001f)
                return;

            UpdatePosition(data.pos);
            UpdateLook(data.verticalLookAngle);
            crouching = data.crouching;
        }

        private void UpdateCharacter(Teams team)
        {
            if (characterView != null)
                return;

            var isRedTeam = team == Teams.Red;
            characterView = isRedTeam ? characterViewRed : characterViewBlue;
            characterViewRed.gameObject.SetActive(isRedTeam);
            characterViewBlue.gameObject.SetActive(!isRedTeam);
        }

        private void JumpToPosition(TransformSnapshot pos)
        {
            transform.position = GetSpawnPos(pos.Pos);
            transform.rotation = Quaternion.Euler(0f, pos.r, 0f);
            movement.Rewind();
        }

        private void UpdatePosition(TransformSnapshot pos)
        {
            if (!posInitialized)
            {
                JumpToPosition(pos);
                posInitialized = true;
                return;
            }

            movement.MoveTo(pos);

            if (!drawDebugPositions)
                return;

            debugPositionSnapshots.Add(pos.Pos);
        }

        private void UpdateLook(float verticalAngle)
        {
            var verticalRotation = Vector3.right * verticalAngle;
            characterView.SetHeadRotation(verticalRotation);
            characterView.SetWeaponRotation(verticalRotation);
        }

        private void UpdateWeapon(long weaponId)
        {
            if (selectedWeaponId == weaponId) return;
            selectedWeaponId = weaponId;
            SetWeapon(weaponData.GetData(selectedWeaponId).otherPrefab);
        }

        private void UpdateAliveState(bool aliveState)
        {
            if (aliveState == alive)
                return;

            if (aliveState) posInitialized = false;
            alive = aliveState;

            characterView.SetAlive(aliveState);
            hitbox.enabled = aliveState;
        }

        private static Vector3 GetSpawnPos(Vector3 pos) =>
            Physics.Raycast(pos, Vector3.down, out var hit)
                ? hit.point
                : pos;

        private void SetWeapon(AssetReferenceGameObject prefabRef)
        {
            setWeaponHandler.Dispose();
            setWeaponHandler = prefabRef
                .LoadPrefabObservable()
                .Subscribe(characterView.SetWeapon)
                .AddTo(this);
        }
    }
}