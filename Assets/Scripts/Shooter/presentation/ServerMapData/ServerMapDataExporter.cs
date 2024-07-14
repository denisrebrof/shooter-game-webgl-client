using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModestTree;
using UnityEngine;

namespace Shooter.presentation.ServerMapData
{
    [ExecuteInEditMode]
    public class ServerMapDataExporter : MonoBehaviour
    {
        [SerializeField] [TextArea(16, 24)] private string export;

        public Color color = Color.red;

        [SerializeField] private float elapsed;
        [SerializeField] private bool update;

        [Header("Debug Info")] [SerializeField]
        private List<List<Transform>> pathPoints;
        [SerializeField, HideInInspector]
        private List<Vector3> expRaycastedPoints;

        private void Update()
        {
            // Update the way to the goal every second.
            elapsed += Time.deltaTime;
            if (elapsed > 1.0f && update)
            {
                elapsed -= 1.0f;
                pathPoints = GetChildList(transform)
                    .Select(GetChildList)
                    .ToList();
                export = BuildExport();
            }

            pathPoints?.ForEach(DrawPath);
        }

        private void OnEnable()
        {
            elapsed = 0.0f;
        }

        private void DrawPath(List<Transform> path)
        {
            for (var i = 0; i < path.Count - 1; i++)
                Debug.DrawLine(path[i].position, path[i + 1].position, color);
        }

        private void OnDrawGizmos() {
            Gizmos.color = color;
            for (var i = 0; i < expRaycastedPoints.Count - 1; i++)
                Gizmos.DrawSphere(expRaycastedPoints[i], 0.5f);
        }

        private List<Transform> GetChildList(Transform root)
        {
            var childs = new List<Transform>();
            for (var i = 0; i < root.childCount; i++)
                childs.Add(root.GetChild(i));
            return childs;
        }

        private string BuildExport()
        {
            if (pathPoints.IsEmpty())
                return "listOf()";

            var builder = new StringBuilder();
            builder.AppendLine("listOf(");
            expRaycastedPoints = new List<Vector3>();
            foreach (var path in pathPoints)
            {
                if (path.IsEmpty())
                {
                    builder.AppendLine("listOf(),");
                    continue;
                }

                builder.AppendLine("listOf(");
                path
                    .Select(point => GetPointPos(point.position))
                    .ToList()
                    .ForEach(pos =>
                    {
                        builder.AppendLine($"Transform({pos.x}f, {pos.y}f, {pos.z}f, 0f),");
                        expRaycastedPoints.Add(pos);
                    });
                builder.AppendLine("),");
            }

            builder.AppendLine(")");
            return builder.ToString();
        }

        private Vector3 GetPointPos(Vector3 p)
        {
            if (!Physics.Raycast(p, Vector3.down, out var hit))
                return p;

            return hit.point;
        }
    }
}