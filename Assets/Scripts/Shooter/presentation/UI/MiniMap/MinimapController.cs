using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Shooter.presentation.UI.MiniMap
{
    public class MinimapController : MonoBehaviour, IMinimapItemHandler
    {
        [SerializeField] private Material mat;
        [SerializeField] private int maxPlayers = 16;
        [SerializeField] private Rect mapBounds;

        private readonly List<Transform> enemyPositions = new();
        private readonly List<Transform> alliePositions = new();


        [CanBeNull] private Transform player;

        private float[] positions;

        public void SetPlayer(Transform pos)
        {
            player = pos;
        }

        public void Add(Transform pos, bool isEnemy)
        {
            var target = isEnemy ? enemyPositions : alliePositions;
            target.Add(pos);
        }

        public void Remove(Transform pos, bool isEnemy)
        {
            var target = isEnemy ? enemyPositions : alliePositions;
            target.Remove(pos);
        }

        private void Awake()
        {
            positions = new float[maxPlayers * 2];
        }

        private Vector2 GetRelativePos(Vector3 worldPos)
        {
            if (mapBounds.width <= 0f || mapBounds.height <= 0f)
                return Vector2.zero;

            var pos = new Vector2(worldPos.x, worldPos.z);
            var innerPos = pos - mapBounds.min;
            var relativePosX = Mathf.Clamp01(innerPos.x / mapBounds.width);
            var relativePosY = Mathf.Clamp01(innerPos.y / mapBounds.height);
            return new Vector2(relativePosX, relativePosY);
        }

        private void Update()
        {
            var positionsIndex = 0;
            var enemiesCount = 0;
            var maxEnemies = Math.Min(maxPlayers, enemyPositions.Count);
            for (var enemyIndex = 0; enemyIndex < maxEnemies; enemyIndex++)
            {
                var pos = GetRelativePos(enemyPositions[enemyIndex].position);
                positions[positionsIndex++] = pos.x;
                positions[positionsIndex++] = pos.y;
                enemiesCount++;
            }

            var alliesCount = 0;
            var maxAllies = Math.Min(maxPlayers, alliePositions.Count);
            for (var allieIndex = 0; allieIndex < maxAllies; allieIndex++)
            {
                var pos = GetRelativePos(alliePositions[allieIndex].position);
                positions[positionsIndex++] = pos.x;
                positions[positionsIndex++] = pos.y;
                alliesCount++;
            }

            mat.SetFloatArray("_EnemiesPos", positions);
            mat.SetFloat("_EnemiesCount", enemiesCount + alliesCount);
            mat.SetFloat("_AllyStartIndex", enemiesCount);

            if (player == null)
                return;

            var playerPos = GetRelativePos(player.position);
            mat.SetFloat("_PlayerPosX", playerPos.x);
            mat.SetFloat("_PlayerPosY", playerPos.y);
            mat.SetFloat("_PlayerRot", player.rotation.eulerAngles.y * Mathf.Deg2Rad);
        }

        private void OnDrawGizmosSelected()
        {
            var center = new Vector3(mapBounds.center.x, -10f, mapBounds.center.y);
            var size = new Vector3(mapBounds.width, 1f, mapBounds.height);
            Gizmos.DrawCube(center, size);
        }
    }
}