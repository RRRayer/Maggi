using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using System.Threading.Tasks;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "GeneratorIdleAction", menuName = "State Machines/Actions/Generator Idle Action")]
public class GeneratorIdleActionSO : StateActionSO
{
    protected override StateAction CreateAction() => new GeneratorIdleAction();
    public InputReader InputReader;
    [Tooltip("발전기가 작동하기 위해 필요한 탭 횟수")]
    public int MaxTapCount;
    [Tooltip("탭 카운트가 감소하는 시간 간격 (초 단위)")]
    public float TapDecreaseInterval;
    [Tooltip("연속적인 탭 입력을 무효화하기 위한 디바운싱 시간 (초 단위)")]
    public float TapDebounceTime;
}

public class GeneratorIdleAction : StateAction
{
    private GeneratorIdleActionSO _originSO => (GeneratorIdleActionSO)base.OriginSO;
    private InputReader _InputReader => _originSO.InputReader;
    private int _maxTapCount => _originSO.MaxTapCount;
    private float _tapDecreaseInterval => _originSO.TapDecreaseInterval;
    private float _tapDebounceTime => _originSO.TapDebounceTime;
    private int _currentTapCount = 0;
    private float _lastTapTime = 0f;
    public bool IsGeneratorInactive { get; private set; } = true;


    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
    }
    public override void OnUpdate()
    {
        // 마우스 좌클릭 시 카운트 증가
        if (IsGeneratorInactive &&
            _InputReader.LeftMouseDown() &&
            (Time.time - _lastTapTime > _tapDebounceTime))
        {
            _currentTapCount++;
            _lastTapTime = Time.time;
#if DEBUG
            Debug.Log($"TapCount: {_currentTapCount} _ GeneratorIdleActionSO.cs");
#endif
        }

        // 일정 시간 입력이 없으면 카운트 감소
        if (IsGeneratorInactive &&
            _currentTapCount > 0 &&
            Time.time - _lastTapTime > _tapDecreaseInterval)
        {
            _currentTapCount--;
            _lastTapTime = Time.time;
#if DEBUG
            Debug.Log($"TapCount: {_currentTapCount} _ GeneratorIdleActionSO.cs");
#endif
        }

        // 발전기 작동
        if (IsGeneratorInactive &&
            _currentTapCount == _maxTapCount)
        {
            IsGeneratorInactive = false;
            Debug.Log("발전기 작동!!!!");
        }
    }
    public override void OnStateEnter()
    {
        _currentTapCount = 0;
        _lastTapTime = Time.time;
        IsGeneratorInactive = true;
    }

    public override void OnStateExit()
    {
    }
}