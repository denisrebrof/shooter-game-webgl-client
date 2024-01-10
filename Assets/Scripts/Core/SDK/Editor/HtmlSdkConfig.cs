using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Editor;

namespace Core.SDK.Editor
{
    [CreateAssetMenu(menuName = "HTML SDK Config", fileName = "HTML SDK Config")]
    public class HtmlSdkConfig : ScriptableObject
    {
        private const string GoogleAnalyticsIdKey = "$GA_ID";
        private const string ProjectTitleKey = "$PROJECT_TITLE";
        private const string MetrikaKey = "$METRIKA_ID";

        public string pageTitle;
        public string googleAnalyticsID;
        public string yandexMetrikaID;

        [SerializeField] private SerializableDictionary<string, string> customDefinitions = new();

        public Dictionary<string, string> Definitions
        {
            get
            {
                var definitions = new Dictionary<string, string>(customDefinitions)
                {
                    [ProjectTitleKey] = pageTitle,
                    [GoogleAnalyticsIdKey] = googleAnalyticsID,
                    [MetrikaKey] = yandexMetrikaID
                };
                return definitions
                    .Where(IsValid)
                    .ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value
                    );
            }
        }

        private bool IsValid(KeyValuePair<string, string> definition)
        {
            return definition.Key.Length > 0 && definition.Value.Length > 0;
        }

        public static bool FindConfig(out HtmlSdkConfig config)
        {
            config = null;
            var configs = SOEditorUtils.GetAllInstances<HtmlSdkConfig>();
            if (!configs.Any()) return false;
            config = configs.First();
            return true;
        }
    }
}