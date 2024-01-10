using System.Collections.Generic;
using System.Linq;
using Core.SDK.SDKType;
using UnityEditor;
using Utils.Editor;

namespace Core.SDK.Editor
{
    public class SwitchSDKPlatform : DefineSymbolsFieldController<SDKProvider.SDKType>
    {
        private static Dictionary<SDKProvider.SDKType, SDKSettingsData> Settings => new()
        {
            {
                SDKProvider.SDKType.Yandex, new SDKSettingsData
                {
                    Symbols = "YANDEX_SDK",
                    Compression = WebGLCompressionFormat.Brotli,
                    DecompressionFallbackEnabled = true
                }
            },
            {
                SDKProvider.SDKType.Vk, new SDKSettingsData
                {
                    Symbols = "VK_SDK",
                    Compression = WebGLCompressionFormat.Disabled,
                    DecompressionFallbackEnabled = false
                }
            },
            {
                SDKProvider.SDKType.Crazy, new SDKSettingsData
                {
                    Symbols = "CRAZY_SDK",
                    Compression = WebGLCompressionFormat.Gzip,
                    DecompressionFallbackEnabled = false //Check?
                }
            },
            {
                SDKProvider.SDKType.Poki, new SDKSettingsData
                {
                    Symbols = "POKI_SDK",
                    Compression = WebGLCompressionFormat.Gzip, //Check?
                    DecompressionFallbackEnabled = false //Check?
                }
            },
            {
                SDKProvider.SDKType.None, new SDKSettingsData
                {
                    Symbols = "",
                    Compression = WebGLCompressionFormat.Disabled,
                    DecompressionFallbackEnabled = false
                }
            }
        };

        protected override string GetSymbols(SDKProvider.SDKType type) => Settings[type].Symbols;

        protected override string[] GetAvailableSymbols() => Settings
            .Values
            .Select(item => item.Symbols)
            .ToArray();


        [MenuItem("Platform/Set Yandex", false)]
        public static void SetYandex() => SetSDKType(SDKProvider.SDKType.Yandex);

        [MenuItem("Platform/Set VK", false)]
        public static void SetVk() => SetSDKType(SDKProvider.SDKType.Vk);

        [MenuItem("Platform/Set Poki", false)]
        public static void SetPoki() => SetSDKType(SDKProvider.SDKType.Poki);

        [MenuItem("Platform/Set Crazy", false)]
        public static void SetCrazy() => SetSDKType(SDKProvider.SDKType.Crazy);

        [MenuItem("Platform/Set None", false)]
        public static void SetNone() => SetSDKType(SDKProvider.SDKType.None);

        private static void SetSDKType(SDKProvider.SDKType type)
        {
            new SwitchSDKPlatform().SetVariant(type);
            var settings = Settings[type];
            PlayerSettings.WebGL.compressionFormat = settings.Compression;
            PlayerSettings.WebGL.decompressionFallback = settings.DecompressionFallbackEnabled;
        }

        private struct SDKSettingsData
        {
            public string Symbols;
            public bool DecompressionFallbackEnabled;
            public WebGLCompressionFormat Compression;
        }
    }
}