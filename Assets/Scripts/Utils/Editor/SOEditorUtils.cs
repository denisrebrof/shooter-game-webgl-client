using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    // ReSharper disable once InconsistentNaming
    public static class SOEditorUtils
    {
        public static List<T> GetAllInstances<T>() where T : ScriptableObject => AssetDatabase
            .FindAssets("t:" + typeof(T).Name)
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<T>)
            .ToList();
        
        public static List<ScriptableObject> GetAllInstances(Type soType) => AssetDatabase
            .FindAssets("t:" + soType.Name)
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(assetPath => AssetDatabase.LoadAssetAtPath(assetPath, soType))
            .OfType<ScriptableObject>()
            .ToList();
    }
}