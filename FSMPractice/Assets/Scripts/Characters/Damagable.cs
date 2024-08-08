using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    [Header("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO _onDieEvent = default;

    [HideInInspector] public bool IsDead = false;

    public void Die()
    {
        IsDead = true;

        if (_onDieEvent != null)
        {
            _onDieEvent.RaiseEvent();
        }
    }
}
