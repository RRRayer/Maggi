using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode
{
    Idle, Walk, Detect, Trigger, Catch
}

public class Boss : MonoBehaviour
{
    [SerializeField] private Mode _currentMode = Mode.Idle;
    public Mode CurrentMode {  get { return _currentMode; } }

    // patrol area에 저장된 위치로 walk 한다.
    public Transform[] patrolAreas;

    private int _currentAreaIndex;
    public int CurrentAreaIndex { set { _currentAreaIndex = value; } get { return _currentAreaIndex; } }

    private float _timer;
    private float _patrolCooltime;

    public void SetMode(Mode newMode)
    {
        _currentMode = newMode;
        Debug.Log($"현재 모드 : {newMode}");
    }
}
