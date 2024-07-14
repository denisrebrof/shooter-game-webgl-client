using System;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Utils.Reactive
{
    public static class AddressableRxExtensions
    {
        public static IObservable<bool> LoadSceneObservable(
            this AssetReference sceneRef,
            Action<float> progress
        ) =>
            Observable.Create<bool>(observer =>
            {
                var handle = Addressables.LoadSceneAsync(sceneRef, LoadSceneMode.Additive);
                var progressHandle = Observable
                    .EveryUpdate()
                    .Select(_ => handle.PercentComplete)
                    .Subscribe(progress.Invoke);
                handle.Completed += h =>
                {
                    observer.OnNext(true);
                    observer.OnCompleted();
                };
                handle.Destroyed += h =>
                {
                    observer.OnNext(false);
                    observer.OnCompleted();
                };
                return Disposable.Create(() =>
                {
                    progressHandle.Dispose();
                    // handle.Task.Dispose(); TODO
                    Addressables.Release(handle);
                });
            });

        public static IObservable<GameObject> LoadPrefabObservable(this AssetReferenceGameObject reference) =>
            Observable.Create<GameObject>(observer =>
            {
                AsyncOperationHandle<GameObject> handle;
                
                var op = reference.OperationHandle;
                if (op.IsValid()) {
                    handle = op.Convert<GameObject>();
                    if (handle.IsDone) {
                        if (handle.Result != null) observer.OnNext(handle.Result);
                        observer.OnCompleted();
                        return Disposable.Empty;
                    }
                }
                else {
                    handle = reference.LoadAssetAsync();
                }
                
                handle.Completed += h =>
                {
                    if (h.Result != null) observer.OnNext(h.Result);
                    observer.OnCompleted();
                };
                handle.Destroyed += h => { observer.OnCompleted(); };
                return Disposable.Empty; //TODO
            });
    }
}