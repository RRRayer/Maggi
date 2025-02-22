using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsPullingNormalObjectConditionSO", menuName = "State Machines/Conditions/Is Pulling Normal Object Condition")]
public class IsPullingNormalObjectConditionSO : StateConditionSO<IsPullingNormalObjectCondition> { }

public class IsPullingNormalObjectCondition : Condition
{
    private InteractionManager _interactionManager;

    public override void Awake(StateMachine stateMachine)
    {
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    protected override bool Statement()
    {
        if (_interactionManager.currentInteractionType == InteractionType.Normal)
            return true;
        return false;
    }
}
