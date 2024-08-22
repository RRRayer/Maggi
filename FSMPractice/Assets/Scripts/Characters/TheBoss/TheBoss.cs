using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBoss : MonoBehaviour
{
    [SerializeField] private Transform _appearTransform;
    [SerializeField] private Transform _shelf001Transform;
    [SerializeField] private Transform _jollyChimpTransform;
    [SerializeField] private Transform _shelf002Transform;

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

    public void SpawnShelf001()
    {
        _bossTransform.position = _shelf001Transform.position;
        _bossTransform.rotation = _shelf001Transform.rotation;
    }

    public void SpawnJollyChimp()
    {
        _bossTransform.position = _jollyChimpTransform.position;
        _bossTransform.rotation = _jollyChimpTransform.rotation;
    }
    public void SpawnShelf002()
    {
        _bossTransform.position = _shelf002Transform.position;
        _bossTransform.rotation = _shelf002Transform.rotation;
    }
}
