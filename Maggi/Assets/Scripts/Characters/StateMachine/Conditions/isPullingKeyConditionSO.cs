using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "isPullingKeyCondition", menuName = "State Machines/Conditions/is Pulling Key Condition")]
public class isPullingKeyConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new isPullingKeyCondition();
}

public class isPullingKeyCondition : Condition
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
