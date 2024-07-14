using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Core.LazyResources.presentation.Editor
{
    public static class LazyResBundleUtils
    {
        public static void BuildEditorBundle(
            LazyResConfig config
        )
        {
            var build = new AssetBundleBuild
            {
                assetBundleName = config.bundleName,
                assetNames = config
                    .TextureData
                    .Values
                    .Select(data => AssetDatabase.GetAssetPath(data.texture))
                    .ToArray()
            };

            var buildFolder = Path.Combine("Assets", config.editorBundleFolder);
            if (!AssetDatabase.IsValidFolder(buildFolder))
            {
                AssetDatabase.CreateFolder("Assets", config.editorBundleFolder);
                Debug.Log($"Created folder \"{buildFolder}\" for built bundles");
            }

            var builds = new[] { build };
            BuildPipeline.BuildAssetBundles(
                buildFolder,
                builds,
                BuildAssetBundleOptions.None,
                EditorUserBuildSettings.activeBuildTarget
            );
            AssetDatabase.Refresh();
        }
    }
}