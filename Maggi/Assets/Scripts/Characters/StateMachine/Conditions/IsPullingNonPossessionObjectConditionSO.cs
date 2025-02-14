using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsPullingNonPossessionObjectCondition", menuName = "State Machines/Conditions/Is Pulling Non Possession Object Condition")]
public class IsPullingNonPossessionObjectConditionSO : StateConditionSO<IsPullingNonPossessionObjectCondition> { }

public class IsPullingNonPossessionObjectCondition : Condition
{
    private InteractionManager _interactionManager;

    public override void Awake(StateMachine stateMachine)
    {
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    protected override bool Statement()
    {
        if (_interactionManager.currentInteractionType == InteractionType.NonPossession)
            return true;
        return false;
    }
}
