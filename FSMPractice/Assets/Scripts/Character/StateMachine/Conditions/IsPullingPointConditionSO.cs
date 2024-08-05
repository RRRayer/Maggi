using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsPullingPointCondition", menuName = "State Machines/Conditions/Is Pulling Point Condition")]
public class IsPullingPointConditionSO : StateConditionSO<IsPullingPointCondition> {}

public class IsPullingPointCondition : Condition
{
	private InteractionManager _interactionManager;

	public override void Awake(StateMachine stateMachine)
	{
		_interactionManager = stateMachine.GetComponent<InteractionManager>();
	}
	
	protected override bool Statement()
	{
		if (_interactionManager.currentInteractionType == InteractionType.Point)
            return true;
        return false;
	}
}
