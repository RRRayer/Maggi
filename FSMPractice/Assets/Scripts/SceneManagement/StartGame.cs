using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameSceneSO _locationsToLoad;
    [SerializeField] private SaveLoadSystem _saveLoadSystem = default;

    [Header("Broadcasting on")]
    [SerializeField] private LoadEventChannelSO _loadLocation = default;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _onStartGame = default;

    private bool _hasSaveData;

    private void Start()
    {
        _hasSaveData = _saveLoadSystem.LoadSaveDataFromDisk();
        _onStartGame.OnEventRaised += StartNewGame;
    }

    private void OnDestroy()
    {
        _onStartGame.OnEventRaised -= StartNewGame;
    }

    private void StartNewGame()
    {
        // 프로토타입 단계에서는 재시작하면 처음부터 시작
        _hasSaveData = false;

        _saveLoadSystem.WriteEmptySaveFile();
        _saveLoadSystem.SetNewGameData();

        // 프로토타입 단계에서는 로딩 화면은 구현하지 않음
        _loadLocation.RaiseEvent(_locationsToLoad);
    }
}
