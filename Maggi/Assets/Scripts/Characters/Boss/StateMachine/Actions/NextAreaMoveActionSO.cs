using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using UnityEngine.AI;
using Maggi.Character.Boss;

[CreateAssetMenu(fileName = "NextAreaMoveAction", menuName = "State Machines/Actions/Boss/Next Area Move Action")]
public class NextAreaMoveActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new NextAreaMoveAction();
}

public class NextAreaMoveAction : StateAction
{
	protected new NextAreaMoveActionSO _originSO => (NextAreaMoveActionSO)base.OriginSO;
	
	private Boss _boss;
	private NavMeshAgent _agent;
	private Transform[] _patrolAreas;
    private int areaIndex = 0;

    public override void Awake(StateMachine stateMachine)
	{
		_boss = stateMachine.GetComponent<Boss>();
		_agent = stateMachine.GetComponent<NavMeshAgent>();
	}

    public override void OnStateEnter()
    {
		_patrolAreas = _boss.patrolAreas[_boss.CurrentRootIndex];
    }

    public override void OnUpdate()
	{
		// move to next area
		_agent.SetDestination(_patrolAreas[areaIndex].position);

        // 에이전트가 목적지까지의 경로를 계산 중이 아니라면(pathPending == false),
        // 남은 거리가 멈출 거리(stoppingDistance) 이하이고,
        // 추가로 velocity가 거의 없을 때 (속도가 0에 가까울 때)
        if (!_agent.pathPending &&
            _agent.remainingDistance <= _agent.stoppingDistance &&
            _agent.velocity.sqrMagnitude < 0.01f)
        {
            areaIndex = (areaIndex + 1) % _patrolAreas.Length;
			_boss.SetMode(Mode.Idle);
        }
    }

    public override void OnStateExit()
	{
	}
}
