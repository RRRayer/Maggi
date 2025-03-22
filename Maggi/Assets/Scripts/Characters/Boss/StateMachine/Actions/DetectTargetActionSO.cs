using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using UnityEngine.AI;
using Maggi.Character.Boss;

[CreateAssetMenu(fileName = "DetectTargetAction", menuName = "State Machines/Actions/Boss/Detect Target Action")]
public class DetectTargetActionSO : StateActionSO<DetectTargetAction> { }

public class DetectTargetAction : StateAction
{
    private Boss _boss;
    private NavMeshAgent _agent;

    public override void Awake(StateMachine stateMachine)
    {
        _boss = stateMachine.GetComponent<Boss>();
        _agent = stateMachine.GetComponent<NavMeshAgent>();
    }

    public override void OnUpdate()
    {
        _agent.SetDestination(_boss.Target.position);

        // Trigger 상태이면 Timeline 실행.
        if (_boss.isTrigger
            && !_agent.pathPending                                      // 경로 계산이 완료되었고
            && _agent.remainingDistance <= _agent.stoppingDistance      // 목표 지점까지 남은 거리가 stoppingDistance 이하이며
            && (!_agent.hasPath || _agent.velocity.sqrMagnitude <= 0f)) // 이동 중이 아니면
        {
            _boss.SetMode(Mode.Trigger);
        }
    }
}
