using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.StringSelector
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class StringSelectorAttribute : PropertyAttribute
    {
        public readonly Dictionary<string, string> DirectEntries;
        public readonly SelectorSourceType SourceType;
        public readonly Type Source;

        private StringSelectorAttribute()
        {
            //Restrict default constructor
        }

        public StringSelectorAttribute(Dictionary<string, string> entries)
        {
            DirectEntries = entries;
            SourceType = SelectorSourceType.Direct;
        }
        
        public StringSelectorAttribute(Type entriesSource)
        {
            Source = entriesSource;
            SourceType = SelectorSourceType.SOSource;
        }

        public enum SelectorSourceType
        {
            Direct,
            SOSource
        }
    }
}