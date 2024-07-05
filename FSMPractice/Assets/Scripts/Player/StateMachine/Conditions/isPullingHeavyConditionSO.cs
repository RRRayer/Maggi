using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "isPullingHeavyCondition", menuName = "State Machines/Conditions/is Pulling Heavy Condition")]
public class isPullingHeavyConditionSO : StateConditionSO<isPullingHeavyCondition> { }

public class isPullingHeavyCondition : Condition
{
    private InteractionManager _interactionManager;

    public override void Awake(StateMachine stateMachine)
    {
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    protected override bool Statement()
    {
        if (_interactionManager.currentInteractionType == InteractionType.Heavy)
            return true;
        return false;
    }
}
