using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField] private Player _playerPrefab = default;
    [SerializeField] private PointStorageSO _pointTaken = default;


    [Header("Scene Ready Event")]
    [SerializeField] private VoidEventChannelSO _onSceneReady = default;

    private SavePoint[] _spawnPoints;
    private Transform _defaultSpawnPoint;

    private void Awake()
    {
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
            return _defaultSpawnPoint;

        int pointIndex = Array.FindIndex(_spawnPoints, element =>
            element.PointPath == _pointTaken.lastPointTaken);

        if (pointIndex == -1)
        {
            Debug.LogWarning("The player tried to spawn in an Save Point that doesn't exist, returning the default one.");
            return _defaultSpawnPoint;
        }
        else
        {
            return _spawnPoints[pointIndex].transform;
        }
    }

    private void SpawnPlayer()
    {
        Debug.Log("Spawn Player");

        Transform spawnPoint = _defaultSpawnPoint; //GetSpawnPoint();
        Player playerInstance = Instantiate(_playerPrefab, spawnPoint.position, spawnPoint.rotation);

        Debug.Log(spawnPoint.transform);
    }
}
