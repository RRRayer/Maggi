using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "isPullingSieveWheelCondition", menuName = "State Machines/Conditions/is Pulling Sieve Wheel Condition")]
public class isPullingSieveWheelConditionSO : StateConditionSO<isPullingSieveWheelCondition> { }

public class isPullingSieveWheelCondition : Condition
{
	private InteractionManager _interactionManager;

	public override void Awake(StateMachine stateMachine)
	{
		_interactionManager = stateMachine.GetComponent<InteractionManager>();
	}
	
	protected override bool Statement()
	{
		if (_interactionManager.currentInteractionType == InteractionType.None)
			return true;
		return false;
	}
}
