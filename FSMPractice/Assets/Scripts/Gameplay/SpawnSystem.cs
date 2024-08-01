using System;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private Player _playerPrefab = default;
    [SerializeField] private TransformAnchor _playerTransformAnchor = default;
    // [SerializeField] private TransformEventChannelSO _playerInstantiatedChannel = default; /* TimelineBinder¿¡¼­ Listen to */
    [SerializeField] private PointStorageSO _pointTaken = default;
    [SerializeField] private CameraSO _currentCamera = default;


    [Header("Scene Ready Event")]
    [SerializeField] private VoidEventChannelSO _onSceneReady = default;

    private SavePoint[] _spawnPoints;
    private Transform _defaultSpawnPoint;

    private void Awake()
    {
        _spawnPoints = GameObject.FindObjectsOfType<SavePoint>();
        _defaultSpawnPoint = transform.GetChild(0);
    }

    private void OnEnable()
    {
        _onSceneReady.OnEventRaised += SpawnPlayer;
    }

    private void OnDisable()
    {
        _onSceneReady.OnEventRaised -= SpawnPlayer;
    }

    private Transform GetSpawnPoint()
    {
        if (_pointTaken == null)
        {
            _currentCamera.index = 0;
            return _defaultSpawnPoint;
        }
            

        int pointIndex = Array.FindIndex(_spawnPoints, element =>
            element.PointPath == _pointTaken.lastPointTaken);

        if (pointIndex == -1)
        {
            Debug.LogWarning("The player tried to spawn in an Save Point that doesn't exist, returning the default one.");
            _currentCamera.index = 0;
            return _defaultSpawnPoint;
        }
        else
        {
            _currentCamera.index = pointIndex;
            return _spawnPoints[pointIndex].transform;
        }
    }

    private void SpawnPlayer()
    {
        Transform spawnPoint = GetSpawnPoint();
        Player playerInstance = Instantiate(_playerPrefab, spawnPoint.position, spawnPoint.rotation);

        // _playerInstantiatedChannel.RaiseEvent(playerInstance.transform);
        _playerTransformAnchor.Provide(playerInstance.transform);

        _inputReader.EnableGameplayInput();
    }
}
