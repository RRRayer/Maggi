using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBoss : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Transform _bossTransform;

    public void Spawn()
    {
        Debug.Log("시그널 발생 " + _targetTransform);
        _bossTransform = _targetTransform;
    }
}
