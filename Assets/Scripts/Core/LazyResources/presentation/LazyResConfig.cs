using System;
using System.Collections.Generic;
using System.Linq;
using Core.LazyResources.presentation.Model;
using UnityEngine;

namespace Core.LazyResources.presentation
{
    [CreateAssetMenu(menuName = "LazyResConfig")]
    public class LazyResConfig : ScriptableObject
    {
        public string bundleName;
        public string abProdAddress;
        public string abDebugAddress;
        public bool useDebug;
        public bool useLocal;
        public string editorBundleFolder;
        public LazyTextureMaxSize defaultSize = LazyTextureMaxSize.Size2048;
        public List<LazyMaterialSettings> materialSettings;

        [SerializeField] private SerializableDictionary<string, LazyTextureData> data;
        
        public string BundleUrl
        {
#if !UNITY_EDITOR
            get { return abProdAddress; }
#else
            get { return useDebug ? abDebugAddress : abProdAddress; }
#endif
        }

        public IDictionary<string, LazyTextureData> TextureData
        {
            get => data;
            set => data = new SerializableDictionary<string, LazyTextureData>(value);
        }

        public Texture[] Textures => data
            .Values
            .Select(textureData => textureData.texture)
            .ToArray();

        public IDictionary<string, LazyTextureData> BuildTextureData()
        {
            var dataMap = new Dictionary<string, LazyTextureData>();
            foreach (var matSettings in materialSettings)
            {
                var material = matSettings.material;
                var materialTextureNames = material.GetTexturePropertyNames();
                foreach (var texPropName in materialTextureNames)
                {
                    var tex = material.GetTexture(texPropName);
                    if (tex == null)
                        continue;

                    var slot = new LazyTextureSlot { propertyName = texPropName, material = material };
                    var textureData = dataMap.TryGetValue(tex.name, out var texData)
                        ? texData
                        : new LazyTextureData
                        {
                            texture = tex,
                            size = matSettings.size == LazyTextureMaxSize.SizeZero ? defaultSize : matSettings.size,
                            slots = Array.Empty<LazyTextureSlot>()
                        };
                    var textureSlots = textureData.slots;
                    textureSlots = textureSlots.Append(slot).ToArray();
                    var size = (LazyTextureMaxSize)Math.Max((int)textureData.size, (int)matSettings.size);
                    dataMap[tex.name] = new LazyTextureData
                    {
                        texture = tex,
                        size = size,
                        slots = textureSlots,
                    };
                }
            }

            return dataMap;
        }

        [ContextMenu("Build lazy res")]
        public void BuildLazyRes()
        {
            TextureData = BuildTextureData();
        }
    }
}