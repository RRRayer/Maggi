using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsFallingCondition", menuName = "State Machines/Conditions/MaggiRun/Is Falling")]
public class IsFallingConditionSO : StateConditionSO
{
    protected override Condition CreateCondition() => new IsFallingCondition();
}

public class IsFallingCondition : Condition
{
    private Rigidbody _rb;

    public override void Awake(StateMachine stateMachine)
    {
        _rb = stateMachine.GetComponent<Rigidbody>();
    }

    protected override bool Statement()
    {
        return _rb.linearVelocity.y < -0.1f;
    }
}