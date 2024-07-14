using System;
using UnityEngine;

namespace Core.LazyResources.presentation.Model
{
    [Serializable]
    public struct LazyMaterialSettings
    {
        public LazyTextureMaxSize size;
        public Material material;
    }

    [Serializable]
    public struct LazyTextureData
    {
        public Texture texture;
        public LazyTextureMaxSize size;
        public LazyTextureSlot[] slots;
    }

    [Serializable]
    public struct LazyTextureSlot
    {
        public Material material;
        public string propertyName;
    }
}