using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PointMoveToAction", menuName = "State Machines/Actions/Point Move To Action")]
public class PointMoveToActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new PointMoveToAction();
}

public class PointMoveToAction : StateAction
{
	protected new PointMoveToActionSO _originSO => (PointMoveToActionSO)base.OriginSO;

	public override void Awake(StateMachine stateMachine)
	{
	}
	
	public override void OnUpdate()
	{
	}
	
	public override void OnStateEnter()
	{
	}
	
	public override void OnStateExit()
	{
	}
}
