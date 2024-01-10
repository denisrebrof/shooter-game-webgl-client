using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using Utils.Editor;

namespace Core.Analytics.Editor
{
    public class SwitchAnalyticsSDK : DefineSymbolsFieldController<AnalyticsType>
    {
        private Dictionary<AnalyticsType, string> Symbols => new()
        {
            { AnalyticsType.None, "" },
            { AnalyticsType.Debug, "DEBUG_ANALYTICS" },
            { AnalyticsType.GameAnalytics, "GAME_ANALYTICS" },
            { AnalyticsType.Playfab, "PLAYFAB_ANALYTICS" },
            { AnalyticsType.Google, "GOOGLE_ANALYTICS" },
        };

        protected override string GetSymbols(AnalyticsType type) => Symbols[type];

        protected override string[] GetAvailableSymbols() => Symbols.Values.ToArray();

        [MenuItem("AnalyticsSDK/Set Game Analytics", false)]
        public static void SetGameAnalytics() => SetSDKType(AnalyticsType.GameAnalytics);

        [MenuItem("AnalyticsSDK/Set Google Analytics", false)]
        public static void SetGoogleAnalytics() => SetSDKType(AnalyticsType.Google);

        [MenuItem("AnalyticsSDK/Set Playfab Analytics", false)]
        public static void SetPlayfabAnalytics() => SetSDKType(AnalyticsType.Playfab);

        [MenuItem("AnalyticsSDK/Set Debug", false)]
        public static void SetDebug() => SetSDKType(AnalyticsType.Debug);

        [MenuItem("AnalyticsSDK/Set None", false)]
        public static void SetNone() => SetSDKType(AnalyticsType.None);

        private static void SetSDKType(AnalyticsType type) => new SwitchAnalyticsSDK().SetVariant(type);
    }
}