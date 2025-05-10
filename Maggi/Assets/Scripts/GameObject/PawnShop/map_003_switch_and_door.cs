using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map_003_switch_and_door : MonoBehaviour
{
    [SerializeField] private Animator _animator = default;
    [SerializeField] private VoidEventChannelSO _onGeneratorActivated;
    [SerializeField] private VoidEventChannelSO _onSwitchPowerOn;

    private bool _blockTrigger = false;

    private bool _isSwitchLocked = true;
    private void Awake()
    {
        _animator??= GetComponent<Animator>();
    }
    private void OnEnable()
    {
        _onGeneratorActivated.OnEventRaised += PowerOnSwitch;
        _onSwitchPowerOn.OnEventRaised += TriggerAnimator;
    }
    private void OnDisable()
    {
        _onGeneratorActivated.OnEventRaised -= PowerOnSwitch;
        _onSwitchPowerOn.OnEventRaised -= TriggerAnimator;
    }

    private void PowerOnSwitch()
    {
        _isSwitchLocked = false;
    }
    private void TriggerAnimator()
    {
        if (_isSwitchLocked || _blockTrigger) return;
        _blockTrigger = true;
        _animator.SetTrigger("switch_down");
    }
}
