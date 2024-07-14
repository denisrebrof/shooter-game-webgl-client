using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class RenameAnimationClips : EditorWindow
{
    [MenuItem("Tools/Rename Animation Clips")]
    public static void ShowWindow()
    {
        GetWindow<RenameAnimationClips>("Rename Animation Clips");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Rename Animation Clips"))
        {
            RenameSelectedFBXAnimations();
        }

        if (GUILayout.Button("Setup Animation Clips"))
        {
            SetupSelectedFBXAnimations();
        }

        if (GUILayout.Button("Export Animation Clips"))
        {
            ExportSelectedFBXAnimations();
        }
    }

    private void SetupSelectedFBXAnimations()
    {
        Object[] selectedObjects = Selection.objects;
        List<string> fbxPaths = new List<string>();

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            if (path.EndsWith(".fbx", StringComparison.OrdinalIgnoreCase))
            {
                fbxPaths.Add(path);
            }
        }

        foreach (string fbxPath in fbxPaths)
        {
            ModelImporter modelImporter = AssetImporter.GetAtPath(fbxPath) as ModelImporter;
            if (modelImporter != null)
            {
                ModelImporterClipAnimation[] clipAnimations = modelImporter.clipAnimations;

                if (clipAnimations.Length == 0)
                {
                    clipAnimations = modelImporter.defaultClipAnimations;
                }

                for (int i = 0; i < clipAnimations.Length; i++)
                {
                    clipAnimations[i].lockRootRotation = true;
                    clipAnimations[i].lockRootHeightY = true;
                    clipAnimations[i].lockRootPositionXZ = true;
                    clipAnimations[i].keepOriginalOrientation = true;
                    clipAnimations[i].keepOriginalPositionY = true;
                    clipAnimations[i].keepOriginalPositionXZ = true;
                }

                modelImporter.clipAnimations = clipAnimations;
                AssetDatabase.WriteImportSettingsIfDirty(fbxPath);
                AssetDatabase.ImportAsset(fbxPath, ImportAssetOptions.ForceUpdate);
            }
        }

        Debug.Log("Animation clips renamed successfully.");
    }

    private void ExportSelectedFBXAnimations()
    {
        Object[] selectedObjects = Selection.objects;
        List<string> fbxPaths = new List<string>();

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            if (path.EndsWith(".fbx", StringComparison.OrdinalIgnoreCase))
            {
                fbxPaths.Add(path);
            }
        }

        Debug.Log("fbxPaths : " + fbxPaths.Count);
        foreach (string fbxPath in fbxPaths)
        {
            ModelImporter modelImporter = AssetImporter.GetAtPath(fbxPath) as ModelImporter;
            var mi = modelImporter != null;
            Debug.Log("MI : " + mi.ToString());
            if (modelImporter != null)
            {
                ModelImporterClipAnimation[] clipAnimations = modelImporter.clipAnimations;

                if (clipAnimations.Length == 0)
                {
                    clipAnimations = modelImporter.defaultClipAnimations;
                }

                string modelName = Path.GetFileNameWithoutExtension(fbxPath);
                string directoryPath = Path.GetDirectoryName(fbxPath);

                // Экспорт анимационного клипа как отдельный ассет
                AnimationClip originalClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(fbxPath);
                AnimationClip newClip = new AnimationClip();
                EditorUtility.CopySerialized(originalClip, newClip);
                newClip.name = modelName;

                string newClipPath = Path.Combine(directoryPath, $"{modelName}_Clip_Exported.anim");
                Debug.Log("Asset created.");
                AssetDatabase.CreateAsset(newClip, newClipPath);
            }
        }

        Debug.Log("Animation clips exported successfully.");
    }

    private void RenameSelectedFBXAnimations()
    {
        Object[] selectedObjects = Selection.objects;
        List<string> fbxPaths = new List<string>();

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            if (path.EndsWith(".fbx", StringComparison.OrdinalIgnoreCase))
            {
                fbxPaths.Add(path);
            }
        }

        foreach (string fbxPath in fbxPaths)
        {
            ModelImporter modelImporter = AssetImporter.GetAtPath(fbxPath) as ModelImporter;
            if (modelImporter != null)
            {
                ModelImporterClipAnimation[] clipAnimations = modelImporter.clipAnimations;

                if (clipAnimations.Length == 0)
                {
                    clipAnimations = modelImporter.defaultClipAnimations;
                }

                string modelName = Path.GetFileNameWithoutExtension(fbxPath);

                for (int i = 0; i < clipAnimations.Length; i++)
                {
                    clipAnimations[i].name = modelName;
                }

                modelImporter.clipAnimations = clipAnimations;
                AssetDatabase.WriteImportSettingsIfDirty(fbxPath);
                AssetDatabase.ImportAsset(fbxPath, ImportAssetOptions.ForceUpdate);
            }
        }

        Debug.Log("Animation clips renamed successfully.");
    }
}