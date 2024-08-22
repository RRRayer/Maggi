using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBoss : MonoBehaviour
{
    [SerializeField] private Transform _appearTransform;
    [SerializeField] private Transform _shelfTransform;
    [SerializeField] private Transform _bossTransform;

    private void Awake()
    {
        _bossTransform = transform;
    }

    public void SpawnAppearence()
    {
        _bossTransform.position = _appearTransform.position;
        _bossTransform.rotation = _appearTransform.rotation;
    }

    public void SpawnShelf()
    {
        _bossTransform.position = _shelfTransform.position;
        _bossTransform.rotation = _shelfTransform.rotation;
    }
}
