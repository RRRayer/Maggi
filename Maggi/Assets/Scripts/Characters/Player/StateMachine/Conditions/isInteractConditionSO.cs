using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "isInteractCondition", menuName = "State Machines/Conditions/is Interact Condition")]
public class isInteractConditionSO : StateConditionSO<isInteractCondition> { }

public class isInteractCondition : Condition
{
    private InteractionManager _interactionManager;

    public override void Awake(StateMachine stateMachine)
    {
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    protected override bool Statement()
	{
        return _interactionManager.currentInteractionType == InteractionType.None;
    }
}
