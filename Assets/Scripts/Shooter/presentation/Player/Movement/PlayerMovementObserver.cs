using System;
using System.Collections.Generic;
using Shooter.domain.Model;
using UniRx;
using UnityEngine;

namespace Shooter.presentation.Player.Movement
{
    public class PlayerMovementObserver : PlayerDataBehaviour
    {
        [SerializeField] private PlayerMovementAgent agent;
        [SerializeField] private LARotation headRotation;
        [SerializeField] private LARotation weaponRotation;

        private IDisposable movementHandler = Disposable.Empty;

        private bool posInitialized;

        [SerializeField] private List<Vector3> debugBos = new();
        [SerializeField] private bool drawDebug;

        private void OnEnable()
        {
            posInitialized = false;
            movementHandler = PlayerDataFlow.Subscribe(UpdateData);
        }

        private void UpdateData(PlayerData data)
        {
            headRotation.targetRotation = Vector3.right * data.verticalLookAngle;
            weaponRotation.targetRotation = Vector3.right * data.verticalLookAngle;
            if (posInitialized)
            {
                agent.MoveTo(data.pos);
                if (drawDebug)
                    debugBos.Add(data.pos.Pos);
                return;
            }

            transform.position = GetSpawnPos(data.pos.Pos);
            transform.rotation = Quaternion.Euler(0f, data.pos.r, 0f);
            agent.Rewind();
            posInitialized = true;
        }

        private static Vector3 GetSpawnPos(
            Vector3 pos
        ) => Physics.Raycast(pos, -Vector3.up, out var hit) ? hit.point : pos;

        private void OnDisable()
        {
            movementHandler.Dispose();
        }

        private void OnDrawGizmos()
        {
            if (!drawDebug)
                return;

            foreach (var pos in debugBos)
            {
                Gizmos.DrawSphere(pos, 0.5f);    
            }
        }
    }
}