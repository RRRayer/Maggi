using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PushPointAction", menuName = "State Machines/Actions/Push Point Action")]
public class PushPointActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new PushPointAction();
}

public class PushPointAction : StateAction
{
	protected new PushPointActionSO _originSO => (PushPointActionSO)base.OriginSO;

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
