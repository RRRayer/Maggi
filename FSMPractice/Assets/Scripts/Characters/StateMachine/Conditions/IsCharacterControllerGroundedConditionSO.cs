using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsCharacterControllerGroundCondition", menuName = "State Machines/Conditions/Is Character Controller Ground Condition")]
public class IsCharacterControllerGroundConditionSO : StateConditionSO<IsCharacterControllerGroundedCondition> { }

public class IsCharacterControllerGroundedCondition : Condition
{
	private CharacterController _characterController;

	public override void Awake(StateMachine stateMachine)
	{
		_characterController = stateMachine.GetComponent<CharacterController>();
	}

	protected override bool Statement() => _characterController.isGrounded;
}
