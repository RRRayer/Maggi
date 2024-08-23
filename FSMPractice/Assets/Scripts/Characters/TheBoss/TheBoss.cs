using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBoss : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    [SerializeField] private Transform _appearTransform;
    [SerializeField] private Transform _shelfTransform;

    private Transform _bossTransform;

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
        _bossTransform.position = _shelfTransform.position;
        _bossTransform.rotation = _shelfTransform.rotation;
    }

    public void StopPlayer()
    {
        _inputReader.DisableAllInput();
    }

    public void ResumePlayer()
    {
        _inputReader.EnableGameplayInput();
    }
}
