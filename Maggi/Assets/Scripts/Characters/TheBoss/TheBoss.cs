using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBoss : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    [SerializeField] private Transform _appearTransform;
    [SerializeField] private Transform _shelfTransform;

    [SerializeField] private GameObject _catchCollider001;
    [SerializeField] private GameObject _catchCollider002;
    [SerializeField] private GameObject _catchCollider002_1;
    [SerializeField] private GameObject _catchCollider003;

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

    public void OnCatchCollider001()
    {
        _catchCollider001.SetActive(true);
    }

    public void OffCatchCollider001()
    {
        _catchCollider001.SetActive(false);
    }

    public void OnCatchCollider002()
    {
        _catchCollider002.SetActive(true);
    }

    public void OffCatchCollider002()
    {
        _catchCollider002.SetActive(false);
    }

    public void OnCatchCollider002_1()
    {
        _catchCollider002_1.SetActive(true);
    }

    public void OffCatchCollider002_1()
    {
        _catchCollider002_1.SetActive(false);
    }

    public void OnCatchCollider003()
    {
        _catchCollider003.SetActive(true);
    }

    public void OffCatchCollider003()
    {
        Debug.Log("a");
        _catchCollider003.SetActive(false);
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