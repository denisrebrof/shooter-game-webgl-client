using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Utils.Editor;

namespace Core.LazyResources.presentation.Editor
{
    public class LazyResBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        private const string MissingConfigLog = "No lazy res config found, further lazy res setup is skipped";
        private const string MultipleConfigsLog = "More than 1 lazy res config found, click here to select used one";
        private const string FilenameClashLog = "Some of lazy textures contains the same name, this is not allowed";

        public void OnPreprocessBuild(BuildReport report)
        {
            var configs = SOEditorUtils.GetAllInstances<LazyResConfig>();

            if (configs.Count == 0)
            {
                Debug.LogWarning(MissingConfigLog);
                return;
            }

            var config = configs.First();
            if (configs.Count < 1)
                Debug.LogWarning(MultipleConfigsLog, config);

            config.TextureData = config.BuildTextureData();
            // LazyResBundleUtils.BuildEditorBundle(config);
            LazyResLockUtils.Lock(config.Textures);

            foreach (var texData in config.TextureData.Values)
            {
                var path = AssetDatabase.GetAssetPath(texData.texture);
                var importer = (TextureImporter)AssetImporter.GetAtPath(path);
                importer.maxTextureSize = (int)texData.size;
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
            }

            // BuildPlayerWindow.RegisterBuildPlayerHandler(InterruptBuildOnFilenameClash);
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            LazyResLockUtils.Release();
        }

        private static void InterruptBuildOnFilenameClash(BuildPlayerOptions options)
        {
            LazyResLockUtils.Release();
            throw new BuildPlayerWindow.BuildMethodException(FilenameClashLog);
        }
    }
}