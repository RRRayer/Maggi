using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsPullingPossessionObjectCondition", menuName = "State Machines/Conditions/Is Pulling Possession Object Condition")]
public class IsPullingPossessionObjectConditionSO : StateConditionSO<IsPullingPossessionObjectCondition> { }

public class IsPullingPossessionObjectCondition : Condition
{
    private InteractionManager _interactionManager;

    public override void Awake(StateMachine stateMachine)
    {
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    protected override bool Statement()
    {
        if (_interactionManager.currentInteractionType == InteractionType.Possession)
            return true;
        return false;
    }
}
