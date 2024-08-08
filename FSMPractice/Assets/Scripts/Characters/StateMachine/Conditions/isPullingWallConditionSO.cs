using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "isPullingWallCondition", menuName = "State Machines/Conditions/is Pulling Wall Condition")]
public class isPullingWallConditionSO : StateConditionSO<isPullingWallCondition> { }

public class isPullingWallCondition : Condition
{
	private InteractionManager _interactionManager;

	public override void Awake(StateMachine stateMachine)
	{
		_interactionManager = stateMachine.GetComponent<InteractionManager>();
	}
	
	protected override bool Statement()
	{
		if (_interactionManager.currentInteractionType == InteractionType.Wall)
			return true;
		return false;
	}
}
