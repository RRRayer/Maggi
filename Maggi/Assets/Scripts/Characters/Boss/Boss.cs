using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Mode
{
    Idle, Walk, Detect, Trigger, Catch
}

public class Boss : MonoBehaviour
{
    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _stageTransition = default;

    [SerializeField] private Mode _currentMode = Mode.Idle;
    [SerializeField] private Transform _target;
    private int _currentRootIndex;
    
    public Mode CurrentMode => _currentMode;
    public int CurrentRootIndex { set { _currentRootIndex = value; } get { return _currentRootIndex; } }
    public Transform Target => _target;

    // patrol area에 저장된 위치로 walk 한다.
    [SerializeField] private List<Transform> patrolAreaRoot;
    public List<Transform[]> patrolAreas = new List<Transform[]>();

    private void Awake()
    {
        foreach (var item in patrolAreaRoot)
        {
            Transform[] children = new Transform[item.childCount];

            for (int i = 0; i < item.childCount; i++)
            {
                children[i] = item.GetChild(i);
            }

            patrolAreas.Add(children);
        }
    }

    private void OnEnable()
    {
        _stageTransition.OnEventRaised += Init;
    }

    private void OnDisable()
    {
        _stageTransition.OnEventRaised -= Init;
    }

    private void Init()
    {
        Debug.Log("보스 초기화");
        _currentMode = Mode.Idle;
        // Patrol next area
        _currentRootIndex = (_currentRootIndex + 1) % patrolAreaRoot.Count;
    }

    public void SetMode(Mode newMode)
    {
        _currentMode = newMode;
        Debug.Log($"현재 모드 : {newMode}");
    }

    public void OnTriggerChangeDetected(bool entered, GameObject obj)
    {
        if (entered && obj.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지");
            _target = obj.transform;
            SetMode(Mode.Detect);
        }
    }
}
