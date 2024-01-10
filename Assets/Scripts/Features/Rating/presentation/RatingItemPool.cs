using Shooter.presentation.UI.Rating;
using UnityEngine;
using Utils.Pooling;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RatingItemPool : MonoPool<RatingItemView>
{
    [ContextMenu("Generate")]
    public void GeneratePool()
    {
        Generate();
#if UNITY_EDITOR
        EditorApplication.MarkSceneDirty();
#endif
    }
}