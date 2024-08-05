using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsDescendingCondition", menuName = "State Machines/Conditions/Is Descending Condition")]
public class IsDescendingConditionSO : StateConditionSO<IsDescendingCondition> { }

public class IsDescendingCondition : Condition
{
	private Player _player;

	public override void Awake(StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<Player>();
	}
	
	protected override bool Statement()
	{
		return _player.movementVector.y <= 0;
	}
}
