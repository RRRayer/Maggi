using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameSceneSO _locationsToLoad;
    [SerializeField] private SaveLoadSystem _saveLoadSystem = default;
    [SerializeField] private PointStorageSO _pointTaken = default;
    [SerializeField] private bool _showLoadScreen = default;

    [Header("Broadcasting on")]
    [SerializeField] private LoadEventChannelSO _loadLocation = default;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _onNewGameButton = default;
    [SerializeField] private VoidEventChannelSO _onContinueButton = default;

    private bool _hasSaveData;

    private void Start()
    {
        _hasSaveData = _saveLoadSystem.LoadSaveDataFromDisk();

        _onNewGameButton.OnEventRaised += StartNewGame;
        _onContinueButton.OnEventRaised += ContinuePreviousGame;
    }

    private void OnDestroy()
    {
        _onNewGameButton.OnEventRaised -= StartNewGame;
        _onContinueButton.OnEventRaised -= ContinuePreviousGame;
    }

    private void StartNewGame()
    {
        _hasSaveData = false;

        _saveLoadSystem.WriteEmptySaveFile();
        _saveLoadSystem.SetNewGameData();

        _loadLocation.RaiseEvent(_locationsToLoad, _showLoadScreen);
    }

    private void ContinuePreviousGame()
    {
        if (_hasSaveData)
        {
            StartCoroutine(LoadSavedGame());
            Debug.Log("Load Saved Game");
        }
        else
        {
            StartNewGame();
            Debug.Log("Load New Game");
        }
    }

    private IEnumerator LoadSavedGame()
    {
        _pointTaken = _saveLoadSystem.saveData._pointStorage;

        var locationGuid = _saveLoadSystem.saveData._locationId;
        var asyncOperationHandle = Addressables.LoadAssetAsync<LocationSO>(locationGuid);

        yield return asyncOperationHandle;

        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            LocationSO locationSO = asyncOperationHandle.Result;
            _loadLocation.RaiseEvent(locationSO, _showLoadScreen);
        }
    }
}
