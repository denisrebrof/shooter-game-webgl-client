using System;
using System.Collections;
using System.IO;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace Utils.AssetBundles
{
    public static class AssetBundleUtils
    {
        public static bool GetBundleFromAssets(
            string folderName,
            string bundleName,
            out AssetBundle bundle
        )
        {
            var bundlePath = Path.Combine(Application.dataPath, folderName, bundleName);
            bundle = AssetBundle.LoadFromFile(bundlePath);
            return bundle != null;
        }

        public static IObservable<AssetBundle> LoadAssetBundle(
            this MonoBehaviour handler,
            string url,
            uint version
        )
        {
            return Observable.Create<AssetBundle>(observer =>
                {
                    var enumerator = LoadAssetBundle(
                        url: url,
                        version: version,
                        result: bundle =>
                        {
                            observer.OnNext(bundle);
                            observer.OnCompleted();
                        },
                        failure: () =>
                        {
                            Debug.LogError("Load Textures Failed");
                            observer.OnCompleted();
                        });
                    var loadingRoutine = handler.StartCoroutine(enumerator);
                    return Disposable.Create(() => handler.StopCoroutine(loadingRoutine));
                }
            );
        }

        private static IEnumerator LoadAssetBundle(
            string url,
            uint version,
            Action<AssetBundle> result,
            Action failure
        )
        {
            var request = UnityWebRequestAssetBundle.GetAssetBundle(url, version, 0);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                failure();
                yield break;
            }

            var bundle = DownloadHandlerAssetBundle.GetContent(request);
            result(bundle);
        }
    }
}