using System;
using System.Collections.Generic;
using System.Linq;
using Utils.Editor;
using static Utils.StringSelector.StringSelectorAttribute;

namespace Utils.StringSelector.Editor
{
    public static class StringSelectorExtensions
    {
        public static SourceState GetSourceState(
            this StringSelectorAttribute attribute
        ) => attribute.SourceType switch
        {
            SelectorSourceType.Direct => SourceState.Valid,
            SelectorSourceType.SOSource => CheckValidSOSource(attribute.Source)
                ? SourceState.Valid 
                : SourceState.Invalid,
            _ => SourceState.Invalid
        };
        
        public static Dictionary<string, string> GetEntries(
            this StringSelectorAttribute attribute
        ) => attribute.SourceType switch
        {
            SelectorSourceType.Direct => attribute.DirectEntries,
            SelectorSourceType.SOSource => GetSOSourceEntries(attribute.Source),
            _ => new Dictionary<string, string>()
        };

        private static bool CheckValidSOSource(Type sourceType)
        {
            var isValidSource = typeof(IStringSelectorSource).IsAssignableFrom(sourceType);
            return isValidSource && FindEntriesSource(sourceType, out _);
        }

        private static Dictionary<string, string> GetSOSourceEntries(Type sourceType)
        {
            var isValidSource = typeof(IStringSelectorSource).IsAssignableFrom(sourceType);
            if(!isValidSource)
                return new Dictionary<string, string>();
            
            return FindEntriesSource(sourceType, out var source)
                ? source.GetSelectorEntries()
                : new Dictionary<string, string>();
        }

        private static bool FindEntriesSource(Type sourceType, out IStringSelectorSource source)
        {
            source = default;
            var sources = SOEditorUtils
                .GetAllInstances(sourceType)
                .OfType<IStringSelectorSource>()
                .ToList();
            if (!sources.Any()) return false;
            source = sources.First();
            return true;
        }
    }
}