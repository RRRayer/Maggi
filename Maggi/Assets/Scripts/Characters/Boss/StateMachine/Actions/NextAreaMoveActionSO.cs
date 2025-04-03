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

        // Stop and set next area
        if (_boss.IsStopped())
        {
            areaIndex = (areaIndex + 1) % _patrolAreas.Length;
            _boss.SetMode(Mode.Idle, "next area move action");
        }
    }
}
