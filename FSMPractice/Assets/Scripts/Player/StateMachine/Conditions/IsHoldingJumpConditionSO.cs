using UnityEngine;
using Test.StateMachine;
using Test.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsHoldingJumpCondition", menuName = "State Machines/Conditions/Is Holding Jump Condition")]
public class IsHoldingJumpConditionSO : StateConditionSO<IsHoldingJumpCondition> { }

public class IsHoldingJumpCondition : Condition
{
	private Player _player;

	public override void Awake(StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<Player>();
	}

	protected override bool Statement() => _player.jumpInput;
}
