using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "NoInteractionCondition", menuName = "State Machines/Conditions/No Interaction Condition")]
public class NoInteractionConditionSO : StateConditionSO<NoInteractionCondition> { }

public class NoInteractionCondition : Condition
{
    private InteractionManager _interactionManager = default;

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
