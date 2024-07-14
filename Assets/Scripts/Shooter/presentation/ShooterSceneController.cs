using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Core.SDK.GameState;
using InfimaGames.LowPolyShooterPack;
using Shooter.data;
using Shooter.domain.Model;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Utils.Reactive;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;
#if YANDEX_SDK
#endif

namespace Shooter.presentation
{
    public class ShooterSceneController : MonoBehaviour
    {
        [Inject] private GameStateNavigator gameStateNavigator;

        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        [SerializeField] private WeaponDataSO weaponData;
        [SerializeField] private AssetReference lobbyRef;
        
        [SerializeField] private AssetReference gameRef0;
        [SerializeField] private AssetReference gameRef1;
        [SerializeField] private AssetReference gameRef2;
        
        [SerializeField] private GameObject loader;
        [SerializeField] private UnityEvent<float> progress;

        private AsyncOperationHandle<SceneInstance> lobbyHandle;
        private AsyncOperationHandle<SceneInstance> gameHandle0;
        private AsyncOperationHandle<SceneInstance> gameHandle1;
        private AsyncOperationHandle<SceneInstance> gameHandle2;

        private Coroutine progressRoutine;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            commandsUseCase
                .Subscribe<ShooterMatchState>(Commands.MatchState)
                .DistinctUntilChanged()
                .Subscribe(matchState =>
                {
                    if (matchState.gameActive) LoadGame(matchState);
                    else LoadLobby();
                })
                .AddTo(this);
        }

        private void LoadGame(ShooterMatchState state)
        {
            if (!state.gameActive)
                return;

            Observable
                .WhenAll(
                    LoadWeaponPrefab(state.primaryWeapon, true),
                    LoadWeaponPrefab(state.secondaryWeapon, false)
                ).Subscribe(_ => LoadGameScene(state.mapId))
                .AddTo(this);
        }

        private IObservable<Unit> LoadWeaponPrefab(
            PlayerWeaponState weaponState,
            bool isPrimary
        ) =>
            weaponData
                .GetData(weaponState.id)
                .playerPrefab
                .LoadPrefabObservable()
                .Do(prefab => SetupWeaponSettings(prefab, weaponState, isPrimary))
                .AsUnitObservable();

        private void SetupWeaponSettings(
            GameObject prefab,
            PlayerWeaponState weaponState,
            bool isPrimary
        )
        {
            var weapon = prefab.GetComponent<Weapon>();
            weapon.id = weaponState.id;
            var settings = weaponState.settings;
            weapon.SetAmmunitionTotal(settings.rounds);
            weapon.damage = settings.damage;
            weapon.roundsPerMinutes = settings.rpm;
            if (isPrimary)
            {
                PlayerDataStorage.PrimaryWeaponPrefab = weapon.gameObject;
                PlayerDataStorage.PrimaryWeaponId = weaponState.id;
            }
            else
            {
                PlayerDataStorage.SecondaryWeaponPrefab = weapon.gameObject;
                PlayerDataStorage.SecondaryWeaponId = weaponState.id;
            }
        }

        private void LoadGameScene(int mapId)
        {
            gameStateNavigator.SetLevelPlayingState(true);
            UnloadScene(ref lobbyHandle);
            switch (mapId)
            {
                case 0:
                    LoadScene(ref gameHandle0, gameRef0);
                    break;
                case 1:
                    LoadScene(ref gameHandle1, gameRef1);
                    break;
                case 2:
                    LoadScene(ref gameHandle2, gameRef2);
                    break;
            }
        }

        private void LoadLobby()
        {
            gameStateNavigator.SetLevelPlayingState(false);
            UnloadScene(ref gameHandle0);
            UnloadScene(ref gameHandle1);
            UnloadScene(ref gameHandle2);
            LoadScene(ref lobbyHandle, lobbyRef);
        }

        private void LoadScene(
            ref AsyncOperationHandle<SceneInstance> sceneHandle,
            AssetReference sceneRef
        )
        {
            if (!sceneHandle.IsValid())
            {
                StartSceneLoading(ref sceneHandle, sceneRef);
                return;
            }

            var alreadyLoaded = sceneHandle.IsValid() && sceneHandle.Status == AsyncOperationStatus.Succeeded;
            if (alreadyLoaded)
                return;

            StartSceneLoading(ref sceneHandle, sceneRef);
        }

        private void StartSceneLoading(
            ref AsyncOperationHandle<SceneInstance> sceneHandle,
            AssetReference sceneRef
        )
        {
            loader.SetActive(true);
            var handle = Addressables.LoadSceneAsync(sceneRef, LoadSceneMode.Additive);
            handle.Completed += _ => { loader.SetActive(false); };
            sceneHandle = handle;
            progress?.Invoke(0f);
            if (progressRoutine != null) StopCoroutine(progressRoutine);
            progressRoutine = StartCoroutine(UpdateProgressEnumerator(handle));
        }

        private IEnumerator UpdateProgressEnumerator(AsyncOperationHandle handle)
        {
            while (handle.IsValid() && !handle.IsDone)
            {
                progress?.Invoke(handle.PercentComplete);
                yield return null;
            }
        }

        private static void UnloadScene(ref AsyncOperationHandle<SceneInstance> sceneHandle)
        {
            if (!sceneHandle.IsValid())
                return;

            if (!sceneHandle.Result.Scene.isLoaded)
            {
                sceneHandle.Task.Dispose();
                return;
            }

            Addressables.UnloadSceneAsync(sceneHandle, UnloadSceneOptions.None);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private struct ShooterMatchState
        {
            public bool gameActive;
            public int mapId;
            public PlayerWeaponState primaryWeapon;
            public PlayerWeaponState secondaryWeapon;
        }
    }
}