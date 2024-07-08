using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.AddressableAssets;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameSceneSO _gameplayScene = default;

    [Header("Listening to")]
    [SerializeField] private LoadEventChannelSO _loadLocation = default;
    [SerializeField] private LoadEventChannelSO _loadMenu = default;

    [Header("Broadcasting on")]
    [SerializeField] private BoolEventChannellSO _toggleLoadingScreen = default; // Loading interface controller
    [SerializeField] private VoidEventChannelSO _onScreenReady = default; // picked up by the SpawnSystem

    private AsyncOperationHandle<SceneInstance> _loadingOperationHandle;
    private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;

    //Parameters coming from scene loading requests
    private GameSceneSO _sceneToLoad;
    private GameSceneSO _currentlyLoadedScene;
    private bool _showLoadingScreen;

    private SceneInstance _gameplayManagerSceneInstance = new SceneInstance();
    private float _fadeDuration = .5f;
    private bool _isLoading = false; //To prevent a new loading request while already loading a new scene

    private void OnEnable()
    {
        _loadLocation.OnLoadingRequested += LoadLocation;
        _loadMenu.OnLoadingRequested += LoadMenu;
    }

    private void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen, bool fadeScreen)
    {
        // Prevent a double-loading
        if (_isLoading) return;

        _sceneToLoad = locationToLoad;
        _showLoadingScreen = showLoadingScreen;
        _isLoading = true;

        // Load GameplayManager first
        if (_gameplayManagerSceneInstance.Scene == null || !_gameplayManagerSceneInstance.Scene.isLoaded)
        {
            _gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
            _gameplayManagerLoadingOpHandle.Completed += OnGameplayManagersLoaded;
        }
        else
        {
            StartCoroutine(UnloadPreviousScene());
        }
    }

    private void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen, bool fadeScreen)
    {
        // Prevent a double-loading
        if (_isLoading) return;

        _sceneToLoad = menuToLoad;
        _showLoadingScreen = showLoadingScreen;
        _isLoading = true;

        // Get rid of the persistent Gameplay manager scene
        if (_gameplayManagerSceneInstance.Scene != null && _gameplayManagerSceneInstance.Scene.isLoaded)
        {
            Addressables.UnloadSceneAsync(_gameplayManagerSceneInstance, true);
        }

        StartCoroutine(UnloadPreviousScene());
    }

    private void OnGameplayManagersLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        _gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

        StartCoroutine(UnloadPreviousScene());
    }

    private IEnumerator UnloadPreviousScene()
    {
        yield return new WaitForSeconds(.5f);

        if (_currentlyLoadedScene != null)
        {
            if (_currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
            {
                _currentlyLoadedScene.sceneReference.UnLoadScene();
            }
        }

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        if (_showLoadingScreen)
        {
            _toggleLoadingScreen.RaiseEvent(true);
        }

        _loadingOperationHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
        _loadingOperationHandle.Completed += OnNewSceneLoaded;
    }

    private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        _currentlyLoadedScene = _sceneToLoad;

        Scene s = obj.Result.Scene;
        SceneManager.SetActiveScene(s);
        LightProbes.TetrahedralizeAsync();

        _isLoading = false;

        if (_showLoadingScreen)
            _toggleLoadingScreen.RaiseEvent(false);

        StartGameplay();
    }

    private void StartGameplay()
    {
        _onScreenReady.RaiseEvent(); // Spawn system will spawn the Player in a gameplay scene
    }
}
