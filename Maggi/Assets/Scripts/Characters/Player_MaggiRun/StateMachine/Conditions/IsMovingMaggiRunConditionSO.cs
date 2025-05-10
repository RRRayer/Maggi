using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsMovingMaggiRunCondition", menuName = "State Machines/Conditions/MaggiRun/Is MaggiRun Moving")]
public class IsMovingMaggiRunConditionSO : StateConditionSO<IsMovingCondition> { }

public class IsMovingMaggiRunCondition : Condition
{
    private PlayerMaggiRun _player;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<PlayerMaggiRun>();
    }

    protected override bool Statement()
    {
        return _player.movementInput.sqrMagnitude > 0.01f;
    }
}