using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsNotMovingCondition", menuName = "State Machines/Conditions/MaggiRun/Is Not Moving")]
public class IsNotMovingConditionSO : StateConditionSO<IsNotMovingCondition> { }

public class IsNotMovingCondition : Condition
{
    private PlayerMaggiRun _player;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<PlayerMaggiRun>();
    }

    protected override bool Statement()
    {
        return _player.movementInput.sqrMagnitude <= 0.01f;
    }
}