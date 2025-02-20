using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.Diagnostics;

public class GhostPlatform : MonoBehaviour
{
    private readonly string PLAYER_TAG = "Player";

    [SerializeField, Range(min: 0f, max: float.MaxValue)] private float _disappearTime = 1f;
    [SerializeField, Range(min: 0f, max: float.MaxValue)] private float _respawnTime = 1f;
    [SerializeField] private bool _canRespawn = true;

    private GameObject _parent;

    private void Awake()
    {
        _parent = transform.parent.gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            Invoke(nameof(Disappear), _disappearTime);
        }
    }

    private void Disappear()
    {
        _parent.SetActive(false);
        if (_canRespawn)
        {
            Invoke(nameof(Respawn), _respawnTime);
        }
    }
    private void Respawn ()
    {
        _parent.SetActive(true);
    }
}
