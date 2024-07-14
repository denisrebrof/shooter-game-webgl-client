using System.Linq;
using UnityEditor;
using Utils.Editor;
using Utils.WebSocketClient._di;

namespace Utils.WebSocketClient.Editor
{
    public static class WebSocketSettingsEditorUtils
    {
        private static WebSocketInstaller Installer =>
            SOEditorUtils
                .GetAllInstances<WebSocketInstaller>()
                .First();
        
        [MenuItem("Web Socket/Select Installer", false)]
        public static void SelectInstaller() => Selection.activeObject = Installer;

        [MenuItem("Web Socket/Select Current Settings", false)]
        public static void SelectCurrent() => Selection.activeObject = Installer.Settings;

        [MenuItem("Web Socket/Select Prod Settings", false)]
        public static void SelectProd() => Selection.activeObject = Installer.prodSettings;

        [MenuItem("Web Socket/Select Stage Settings", false)]
        public static void SelectStage() => Selection.activeObject = Installer.stageSettings;
    }
}