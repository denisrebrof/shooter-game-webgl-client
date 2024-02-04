using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModestTree;
using UnityEngine;

[ExecuteInEditMode]
public class ServerMapDataExporter : MonoBehaviour
{
    [SerializeField, TextArea(minLines: 16, maxLines: 24)]
    private string export;

    public Color color = Color.red;

    [Header("Debug Info")] [SerializeField]
    private List<List<Transform>> pathPoints;

    [SerializeField] private float elapsed = 0.0f;

    void OnEnable()
    {
        elapsed = 0.0f;
    }

    void Update()
    {
        // Update the way to the goal every second.
        elapsed += Time.deltaTime;
        if (elapsed > 1.0f)
        {
            elapsed -= 1.0f;
            pathPoints = GetChildList(transform)
                .Select(GetChildList)
                .ToList();
            export = BuildExport();
        }

        pathPoints?.ForEach(DrawPath);
    }

    private void DrawPath(List<Transform> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
            Debug.DrawLine(path[i].position, path[i + 1].position, color);
    }

    private List<Transform> GetChildList(Transform root)
    {
        var childs = new List<Transform>();
        for (int i = 0; i < root.childCount; i++)
            childs.Add(root.GetChild(i));
        return childs;
    }

    private string BuildExport()
    {
        if (pathPoints.IsEmpty())
            return "listOf()";

        var builder = new StringBuilder();
        builder.AppendLine($"listOf(");
        foreach (var path in pathPoints)
        {
            if (path.IsEmpty())
            {
                builder.AppendLine($"listOf(),");
                continue;
            }

            builder.AppendLine($"listOf(");
            path
                .Select(point => point.position)
                .ToList()
                .ForEach(pos => builder.AppendLine($"Transform({pos.x}f, {pos.y}f, {pos.z}f, 0f),"));
            builder.AppendLine($"),");
        }

        builder.AppendLine($")");
        return builder.ToString();
    }
}