using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "NextAreaMoveAction", menuName = "State Machines/Actions/Next Area Move Action")]
public class NextAreaMoveActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new NextAreaMoveAction();
}

public class NextAreaMoveAction : StateAction
{
	protected new NextAreaMoveActionSO _originSO => (NextAreaMoveActionSO)base.OriginSO;
	private Boss _boss;

	private Transform[] _patrolAreas;

    public override void Awake(StateMachine stateMachine)
	{
		_boss = stateMachine.GetComponent<Boss>();
	}

    public override void OnStateEnter()
    {
		_patrolAreas = _boss.patrolAreas;
    }

    public override void OnUpdate()
	{
		// _patrolAreas[current area index] 로 이동 시켜
		// _boss.CurrentAreaIndex
	}
	
	
	
	public override void OnStateExit()
	{
	}
}
