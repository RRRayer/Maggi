using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsDeadCondition", menuName = "State Machines/Conditions/Is Dead Condition")]
public class IsDeadConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsDeadCondition1();
}

public class IsDeadCondition1 : Condition
{
	private Damagable _damagableScript;

	public override void Awake(StateMachine stateMachine)
	{
		_damagableScript = stateMachine.GetComponent<Damagable>();
	}
	
	protected override bool Statement()
	{
		return _damagableScript.IsDead;
	}
}
