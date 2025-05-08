using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "SlowModeAction", menuName = "State Machines/Actions/MaggiRun/Slow Mode")]
public class SlowModeActionSO : StateActionSO<SlowModeAction> { }

public class SlowModeAction : StateAction
{
    private PlayerMaggiRun _player;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<PlayerMaggiRun>();
    }

    public override void OnStateEnter()
    {
        _player.EnterSlowMode();  // F 입력을 기다리며 슬로우 상태 진입
    }

    public override void OnStateExit()
    {
        _player.ExitSlowMode();  // 상태 빠질 때 정리
    }

    public override void OnUpdate() { }
}