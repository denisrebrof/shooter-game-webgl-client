using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using Utils.AssetBundles;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;
/*
namespace Core.LazyResources.presentation
{
    public class LazyRes : MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        [SerializeField] private LazyResConfig config;
        [SerializeField] private GameObject loader;

        private void Start()
        {
            loader.SetActive(false);
            GetLazyTextures().Subscribe(Apply).AddTo(this);
        }

        private IObservable<Dictionary<string, Texture>> GetLazyTextures()
        {
#if UNITY_EDITOR
            if (config.useLocal)
            {
                if (!GetLocalBundle(out var bundle))
                {
                    Debug.Log("Failed to load AssetBundle!");
                    return Observable.Empty<Dictionary<string, Texture>>();
                }

                var textures = bundle.LoadAllAssets<Texture>().ToDictionary(texture => texture.name);
                return Observable.Return(textures);
            }
#endif
            return commandsUseCase
                .Request<uint>(Commands.ResVersion)
                .Select(version => this.LoadAssetBundle(config.BundleUrl, version))
                .Switch();
        }
    }
}
*/