using System.Collections.Generic;

namespace Utils.StringSelector
{
    public interface IStringSelectorSource
    {
        public Dictionary<string, string> GetSelectorEntries();
    }
}