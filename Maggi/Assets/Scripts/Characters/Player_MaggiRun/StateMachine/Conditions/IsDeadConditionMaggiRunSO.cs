using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsDeadConditionMaggiRun", menuName = "State Machines/Conditions/MaggiRun/Is Maggi Dead")]
public class IsDeadConditionMaggiRunSO : StateConditionSO
{
    protected override Condition CreateCondition() => new IsDeadCondition();
}

public class IsDeadConditionMaggiRun : Condition
{
    private PlayerMaggiRun _player;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<PlayerMaggiRun>();
    }

    protected override bool Statement()
    {
        return _player.IsDead;
    }
}