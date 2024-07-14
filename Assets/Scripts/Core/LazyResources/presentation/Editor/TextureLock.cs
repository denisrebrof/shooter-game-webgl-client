using System;
using UnityEngine.Serialization;

namespace Core.LazyResources.presentation.Editor
{
    [Serializable]
    public struct TextureLocks
    {
        public TextureLockData[] locks;
    }
    
    [Serializable]
    public struct TextureLockData
    {
        public string path;
        [FormerlySerializedAs("maxSize")] [FormerlySerializedAs("resolution")] public int originalMaxSize;
    } 
}