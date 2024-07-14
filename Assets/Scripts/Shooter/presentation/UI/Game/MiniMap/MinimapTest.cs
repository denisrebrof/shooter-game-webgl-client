using System.Collections.Generic;
using UnityEngine;

namespace Shooter.presentation.UI.Game.MiniMap
{
    public class MinimapTest : MonoBehaviour
    {
        [SerializeField] private Material mat;
        [SerializeField] private List<Vector2> enemyPositions;
        [SerializeField] private List<Vector2> allyPositions;

#if UNITY_EDITOR
        [ContextMenu("Add")]
        private void Capture()
        {
            var list = new List<float>();
            foreach (var pos in enemyPositions)
            {
                list.Add(pos.x);
                list.Add(pos.y);
            }

            foreach (var pos in allyPositions)
            {
                list.Add(pos.x);
                list.Add(pos.y);
            }

            mat.SetFloatArray("_EnemiesPos", list);
            mat.SetFloat("_EnemiesCount", enemyPositions.Count + allyPositions.Count);
            mat.SetFloat("_AllyStartIndex", enemyPositions.Count);
        }
#endif
    }
}