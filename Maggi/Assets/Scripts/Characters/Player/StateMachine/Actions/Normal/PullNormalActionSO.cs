using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PullNormalAction", menuName = "State Machines/Actions/Pull Normal Action")]
public class PullNormalActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new PullNormalAction();
}

public class PullNormalAction : StateAction
{
	protected new PullNormalActionSO _originSO => (PullNormalActionSO)base.OriginSO;

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
