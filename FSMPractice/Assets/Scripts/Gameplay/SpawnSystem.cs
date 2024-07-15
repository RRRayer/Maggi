using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    private Transform _defaultSpawnPoint;

    private void Awake()
    {
        _defaultSpawnPoint = transform.GetChild(0);
    }

    private void SpawnPlayer()
    {
        
    }
}
