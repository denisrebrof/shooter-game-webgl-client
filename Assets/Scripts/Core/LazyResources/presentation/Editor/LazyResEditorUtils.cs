using System.Linq;
using UnityEditor;
using Utils.Editor;

namespace Core.LazyResources.presentation.Editor
{
    public static class LazyResEditorUtils
    {
        [MenuItem("Lazy Res/Lock", false)]
        public static void TryLock()
        {
            var configs = SOEditorUtils.GetAllInstances<LazyResConfig>();
            var config = configs.First();
            LazyResLockUtils.Lock(config.Textures);
        }

        [MenuItem("Lazy Res/Restore", false)]
        public static void TryRestore() => LazyResLockUtils.Release();

        [MenuItem("Lazy Res/Select Config", false)]
        public static void SelectConfig()
        {
            var configs = SOEditorUtils.GetAllInstances<LazyResConfig>();
            var config = configs.First();
            Selection.activeObject = config;
        }

        [MenuItem("Lazy Res/Build Textures Bundle", false)]
        public static void BuildEditorBundle()
        {
            var configs = SOEditorUtils.GetAllInstances<LazyResConfig>();
            var config = configs.First();
            LazyResBundleUtils.BuildEditorBundle(config);
        }
    }
}