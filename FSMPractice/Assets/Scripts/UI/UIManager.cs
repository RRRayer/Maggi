using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UISetting _UISetting = default;

    [Header("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO _onStartGame = default;

    private void OnEnable()
    {
        _UISetting.RestartButtonAction += Restart;
    }

    private void OnDisable()
    {
        _UISetting.RestartButtonAction -= Restart;
    }

    private void Restart()
    {
        _onStartGame.RaiseEvent();
    }
}
