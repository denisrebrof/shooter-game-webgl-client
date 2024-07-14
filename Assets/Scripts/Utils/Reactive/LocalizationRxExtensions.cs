using System;
using UniRx;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Utils.Reactive
{
    public static class LocalizationRxExtensions
    {
        public static IObservable<string> GetLocalizedStringObservable(
            this LocalizedString localizedString
        ) =>
            Observable.Create<string>(observer =>
                {
                    var handle = localizedString.GetLocalizedStringAsync();
                    handle.Completed += result =>
                    {
                        observer.OnNext(result.Result);
                        observer.OnCompleted();
                    };
                    return Disposable.Create(() =>
                    {
                        if (handle.IsValid()) Addressables.Release(handle);
                        observer.OnCompleted();
                    });
                }
            );
        
        public static IObservable<string> GetLocalizedStringObservable(
            this TableReference table,
            string key
        ) =>
            Observable.Create<string>(observer =>
                {
                    var handle = LocalizationSettings
                        .StringDatabase
                        .GetLocalizedStringAsync(table, key);

                    handle.Completed += result =>
                    {
                        observer.OnNext(result.Result);
                        observer.OnCompleted();
                    };
                    return Disposable.Create(() =>
                    {
                        if (handle.IsValid()) Addressables.Release(handle);
                        observer.OnCompleted();
                    });
                }
            );
    }
}