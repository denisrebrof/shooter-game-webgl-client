using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace Features.LazyResources.domain
{
    public class LazyResourcesUseCase
    {
        public IObservable<Dictionary<string, Texture>> GetLazyTextures()
        {
#if !UNITY_EDITOR
            return Observable.Empty<Dictionary<string, Texture>>();
#endif
            var bundlePath = Path.Combine(Application.dataPath, "LocalBundle","baseresources");
            var myLoadedAssetBundle = AssetBundle.LoadFromFile(bundlePath);
            if (myLoadedAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                return Observable.Empty<Dictionary<string, Texture>>();
            }

            var textures = myLoadedAssetBundle.LoadAllAssets<Texture>().ToDictionary(texture => texture.name);
            return Observable.Return(textures);
        }

        public IObservable<AudioClip> GetLazyAudio(string clipName)
        {
            return Observable.Empty<AudioClip>();
        }
    }
}