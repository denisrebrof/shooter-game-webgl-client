using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Features.LazyResources.presentation
{
    public class SimpleLazyRes : MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        [SerializeField] private GameObject loader;
        [SerializeField] private List<Material> materials;
        [SerializeField] private string abProdAddress;
        [SerializeField] private string abDebugAddress;
        [SerializeField] private bool useDebug;
        [SerializeField] private bool useLocal;

        private Dictionary<String, Material> matMap;

#if UNITY_EDITOR
        private List<MatTextureRestoreData> restoreData = new();
#endif

        private void Start()
        {
            loader.SetActive(false);
            matMap = materials.ToDictionary(mat => mat.name);
            GetLazyTextures().Subscribe(Apply).AddTo(this);
        }

        public IObservable<Dictionary<string, Texture>> GetLazyTextures()
        {
#if UNITY_EDITOR
            if (useLocal)
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
                .Select(LoadAssetBundle)
                .Switch();
        }

        private bool GetLocalBundle(out AssetBundle bundle)
        {
            var bundlePath = Path.Combine(Application.dataPath, "LocalBundle", "baseresources");
            bundle = AssetBundle.LoadFromFile(bundlePath);
            return bundle != null;
        }

        private IObservable<Dictionary<string, Texture>> LoadAssetBundle(uint version)
        {
            loader.SetActive(true);
            return Observable.Create<Dictionary<string, Texture>>(observer =>
                {
                    var enumerator = LoadAssetBundle(
                        version: version,
                        result: bundle =>
                        {
                            var textures = bundle
                                .LoadAllAssets<Texture>()
                                .ToDictionary(texture => texture.name);
                            observer.OnNext(textures);
                            observer.OnCompleted();
                        },
                        failure: () =>
                        {
                            Debug.LogError("Load Textures Failed");
                            observer.OnCompleted();
                        });
                    var loadingRoutine = StartCoroutine(enumerator);
                    return Disposable.Create(() => StopCoroutine(loadingRoutine));
                }
            );
        }

        IEnumerator LoadAssetBundle(
            uint version,
            Action<AssetBundle> result,
            Action failure
        )
        {
#if !UNITY_EDITOR
            var url = abProdAddress;
#else
            var url = useDebug ? abDebugAddress : abProdAddress;
#endif

            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url, version, 0);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                failure();
                yield break;
            }

            var bundle = DownloadHandlerAssetBundle.GetContent(www);
            result(bundle);
        }

        private void Apply(Dictionary<String, Texture> texs)
        {
            loader.SetActive(false);
            foreach (var (key, value) in texs)
            {
                var texAssetNameParts = key.Split("#");
                if (texAssetNameParts.Length < 2)
                    continue;

                var matNames = texAssetNameParts.Take(texAssetNameParts.Length - 1);
                var texName = texAssetNameParts.Last();

                foreach (var matName in matNames)
                {
                    if (!matMap.ContainsKey(matName))
                        continue;

                    var targetMat = matMap[matName];
#if UNITY_EDITOR
                    var existingTex = targetMat.GetTexture(texName);
                    var data = new MatTextureRestoreData(existingTex, matName, texName);
                    restoreData.Add(data);
#endif
                    targetMat.SetTexture(texName, value);
                }
            }
        }

#if UNITY_EDITOR
        private void OnDestroy()
        {
            foreach (var data in restoreData)
                matMap[data.MatName].SetTexture(data.TexName, data.Tex);
        }
#endif

#if UNITY_EDITOR
        [ContextMenu("Clear Cached")]
        public void ClearCache() => Caching.ClearCache();
#endif

#if UNITY_EDITOR
        private struct MatTextureRestoreData
        {
            public Texture Tex;
            public string MatName;
            public string TexName;

            public MatTextureRestoreData(Texture tex, string matName, string texName)
            {
                Tex = tex;
                MatName = matName;
                TexName = texName;
            }
        }
#endif
    }
}