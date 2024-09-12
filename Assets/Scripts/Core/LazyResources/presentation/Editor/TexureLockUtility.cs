using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Core.LazyResources.presentation.Editor
{
    public static class LazyResLockUtils
    {
        private static string TextureLockFilename = "lazy-tex-lock.json";

        private static string LockPath => Path.Combine(Application.dataPath, TextureLockFilename);

        public static void Lock(Texture[] textures)
        {
            var entries = new List<TextureLockData>();
            foreach (var texture in textures)
            {
                try
                {
                    var path = AssetDatabase.GetAssetPath(texture);
                    var importer = (TextureImporter)AssetImporter.GetAtPath(path);
                    var originalSize = importer.maxTextureSize;
                    var entry = new TextureLockData
                    {
                        path = path,
                        originalMaxSize = originalSize
                    };
                    entries.Add(entry);
                }
                catch (Exception)
                {
                    Debug.LogError("Error while create texture lock");
                }
            }

            var locks = new TextureLocks { locks = entries.ToArray() };
            var lockContent = JsonUtility.ToJson(locks);

            File.WriteAllText(LockPath, lockContent);
        }

        public static void Release()
        {
            if (!File.Exists(LockPath))
            {
                Debug.LogError("Lock not found!");
                return;
            }

            var lockFileContent = File.ReadAllText(LockPath);
            var lockData = JsonUtility.FromJson<TextureLocks>(lockFileContent);
            foreach (var textureLock in lockData.locks)
            {
                try
                {
                    var importer = (TextureImporter)AssetImporter.GetAtPath(textureLock.path);
                    var prevMaxSize = importer.maxTextureSize;
                    importer.maxTextureSize = textureLock.originalMaxSize;
                    EditorUtility.SetDirty(importer);
                    importer.SaveAndReimport();
                    Debug.Log($"Restored texture {textureLock.path} from size {prevMaxSize} with size {textureLock.originalMaxSize}");
                }
                catch (Exception)
                {
                    Debug.LogError("Error while restore texture lock");
                }
            }

            Debug.Log("Textures Restored");
            File.Delete(LockPath);
        }
    }
}